using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MyBank.Common.Business.Validation.Attributes;

namespace MyBank.Common.Entities;

[Table ("Employee", Schema = "dbo")]
public sealed class Employee
	: PersonBase
{
	[Key]
	[DatabaseGenerated (DatabaseGeneratedOption.Identity)]
	[DisplayName ("Employee ID")]
	[DisplayFormat (DataFormatString = "{0:D10}", ApplyFormatInEditMode = true)]
	public int ID { get; set; }

	// Navigation Properties
	public EmployeeLoginInfo? LoginInfo { get; set; } = null!;

	public ICollection<Role> Roles { get; set; } = [];

	public ICollection<EmployeesAndRoles> EmployeeRoles { get; set; } = [];
}