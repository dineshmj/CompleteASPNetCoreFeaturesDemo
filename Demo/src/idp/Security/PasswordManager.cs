using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Identity;

using MyBank.Common.Entities;

namespace MyBank.IDP.Security;

public sealed class PasswordManager
    : IPasswordManager
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string HashPassword(EmployeeLoginInfo user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(EmployeeLoginInfo user, string storedHash, string providedPassword)
    {
		using (var sha256 = SHA256.Create ())
		{
			// Hash the incoming password "bob123"
			var bytes = sha256.ComputeHash (Encoding.UTF8.GetBytes (providedPassword));

			// Convert to Hex string (matching the '2' style in SQL CONVERT)
			var hash = BitConverter.ToString (bytes).Replace ("-", "").ToLower ();

			// Compare (Case-insensitive)
			return hash.Equals (storedHash, StringComparison.OrdinalIgnoreCase);
		}
    }
}