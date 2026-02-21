using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.Common.Entities;

[Table("Role", Schema = "dbo")]
public sealed class Role
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [MaxLength(50)]
    [DisplayName ("Role Name")]
		public string RoleName { get; set; } = null!;

    [Required]
    [DefaultValue(true)]
    [DisplayName ("Is Active?")]
    public bool IsActive { get; set; } = true;

    public ICollection<Employee> Employees { get; set; } = [];

	public ICollection<EmployeesAndRoles> EmployeeRoles { get; set; } = [];
}