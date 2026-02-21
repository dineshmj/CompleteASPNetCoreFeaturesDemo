using MyBank.Common.Entities;

namespace MyBank.IDP.DBAccess;

public interface IUserRepository
{
    Task<EmployeeLoginInfo?> FindByUsernameAsync(string username);

    Task<Employee?> FindBySubjectIdAsync(string subjectId);

    Task<IEnumerable<string>> GetRolesByUserIDAsync(int userId);

    Task<bool> ValidateCredentialsAsync(string username, string password);
}