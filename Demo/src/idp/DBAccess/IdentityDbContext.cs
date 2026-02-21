using Microsoft.EntityFrameworkCore;

using MyBank.Common.Entities;

namespace MyBank.IDP.DBAccess;

public sealed class IdentityDbContext
    : DbContext
{
	public DbSet<Employee> Employees { get; set; }

	public DbSet<Role> Roles { get; set; }

	public DbSet<EmployeeLoginInfo> EmployeeLoginInfos { get; set; }

	public DbSet<EmployeesAndRoles> EmployeesAndRoles { get; set; }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
		// Define the Composite Primary Key for the Junction Table
		modelBuilder.Entity<EmployeesAndRoles> ()
			.HasKey (er => new { er.EmployeeID, er.RoleID });

		modelBuilder.Entity<EmployeesAndRoles> ()
			.HasOne (er => er.Employee)
			.WithMany (e => e.EmployeeRoles)
			.HasForeignKey (er => er.EmployeeID);

		modelBuilder.Entity<EmployeesAndRoles> ()
			.HasOne (er => er.Role)
			.WithMany (r => r.EmployeeRoles)
			.HasForeignKey (er => er.RoleID);

		// 2. EmployeeLoginInfo: One-to-One relationship configuration
		modelBuilder.Entity<EmployeeLoginInfo> ()
			.HasKey (li => li.UserName);

		modelBuilder.Entity<EmployeeLoginInfo> ()
			.HasOne (li => li.Employee)
			.WithOne (e => e.LoginInfo)
			.HasForeignKey<EmployeeLoginInfo> (li => li.EmployeeID);

		// 3. Optional: Configure IsActive default value to match SQL script
		modelBuilder.Entity<Role> ()
			.Property (r => r.IsActive)
			.HasDefaultValue (true);

		base.OnModelCreating (modelBuilder);
	}
}