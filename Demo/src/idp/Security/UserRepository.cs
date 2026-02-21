using Microsoft.EntityFrameworkCore;

using MyBank.Common.Entities;
using MyBank.IDP.Security;

namespace MyBank.IDP.DBAccess;

public sealed class UserRepository
    : IUserRepository
{
    private readonly IdentityDbContext _context;
    private readonly IPasswordManager _passwordManager; // Injected dependency

    public UserRepository(IdentityDbContext context, IPasswordManager passwordManager)
    {
        _context = context;
        _passwordManager = passwordManager;
    }

    public async Task<EmployeeLoginInfo?> FindByUsernameAsync(string username)
    {
		// Get the employee record by linking the EmployeeLoginInfo with the Employee and their roles
        var employee
            = await _context.EmployeeLoginInfos
                .Include(u => u.Employee)
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();

		return employee;
    }

    public async Task<Employee?> FindBySubjectIdAsync(string subjectId)
    {
        if (int.TryParse(subjectId, out int id))
        {
            return await _context.Employees
                .FirstOrDefaultAsync(u => u.ID == id);
        }

        return null;
    }

    public async Task<IEnumerable<string>> GetRolesByUserIDAsync(int userId)
    {
        return await _context.EmployeesAndRoles
            .Where(ur => ur.EmployeeID == userId)
            .Select(ur => ur.Role.RoleName)
            .ToListAsync();
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await _context.EmployeeLoginInfos.Where (eli => eli.UserName == username).FirstOrDefaultAsync ();

        if (user == null)
        {
            return false;
        }

        var isValidUser = _passwordManager.VerifyPassword(user, user.PasswordHash, password);

        return isValidUser;
    }
}