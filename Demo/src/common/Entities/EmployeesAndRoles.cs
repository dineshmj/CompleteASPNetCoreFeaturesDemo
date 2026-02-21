using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.Common.Entities;

[Table ("EmployeesAndRoles", Schema = "dbo")]
public class EmployeesAndRoles
{
	[Key, Column (Order = 0)]
	public int EmployeeID { get; set; }

	[Key, Column (Order = 1)]
	public int RoleID { get; set; }

	// Navigation Properties
	[ForeignKey ("EmployeeID")]
	public virtual Employee Employee { get; set; } = null!;

	[ForeignKey ("RoleID")]
	public virtual Role Role { get; set; } = null!;
}