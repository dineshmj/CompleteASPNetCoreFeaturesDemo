using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.Common.Entities;

[Table ("EmployeeLoginInfo", Schema = "dbo")]
public sealed class EmployeeLoginInfo
{
	[Key]
	[StringLength (50)]
	[DatabaseGenerated (DatabaseGeneratedOption.None)]
	[DisplayName ("User Name")]
	public string UserName { get; set; } = null!;

	[Required]
	[StringLength (250, MinimumLength = 6, ErrorMessage = "Password hash must be between 6 and 250 characters.")]
	public string PasswordHash { get; set; } = null!;

	[Required]
	[ForeignKey ("Employee")]
	[DisplayName ("Employee ID")]
	public int EmployeeID { get; set; }

	// Navigation property
	public Employee Employee { get; set; } = null!;
}