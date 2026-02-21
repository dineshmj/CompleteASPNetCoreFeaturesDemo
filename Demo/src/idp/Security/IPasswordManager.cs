using MyBank.Common.Entities;

namespace MyBank.IDP.Security
{
    public interface IPasswordManager
    {
        string HashPassword(EmployeeLoginInfo user, string password);

        bool VerifyPassword(EmployeeLoginInfo user, string storedHash, string providedPassword);
    }
}