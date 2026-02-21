using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBank.Common.Models;

public sealed class LoginInfoDTO
{
	[Required]
	[DisplayName("User Name")]
	[StringLength(50, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 50 characters.")]
	public string UserName { get; set; } = string.Empty;

	[Required]
	[DisplayName("Password")]
	[DataType(DataType.Password)]
	[StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
	public string Password { get; set; } = string.Empty;
}