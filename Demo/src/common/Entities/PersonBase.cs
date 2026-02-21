using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MyBank.Common.Business.Validation.Attributes;

namespace MyBank.Common.Entities;

public abstract class PersonBase
{
	[Required]
	[DisplayName ("First Name")]
	[StringLength (50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
	public string FirstName { get; set; } = null!;

	[Required]
	[DisplayName ("Last Name")]
	[StringLength (50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
	public string LastName { get; set; } = null!;

	[NotMapped]
	[DisplayName ("Full Name")]
	public string FullName => $"{FirstName} {LastName}".Trim ();

	[Required]
	[DisplayName ("Date of Birth")]
	[DisplayFormat (DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
	[MustBeAnAdult]
	public DateTime DateOfBirth { get; set; }

	[Required]
	[DisplayName ("Email Address")]
	[StringLength (50, ErrorMessage = "Email address cannot exceed 50 characters.")]
	[EmailAddress (ErrorMessage = "Invalid email address format.")]
	public string EmailAddress { get; set; } = null!;

	[Required]
	[DisplayName ("Social Security Number")]
	[StringLength (11, MinimumLength = 11, ErrorMessage = "Social Security Number must be exactly 11 characters in the format XXX-XX-XXXX.")]
	[RegularExpression (@"^\d{3}-\d{2}-\d{4}$", ErrorMessage = "Social Security Number must be in the format XXX-XX-XXXX.")]
	public string SocialSecurityNumber { get; set; } = null!;
}