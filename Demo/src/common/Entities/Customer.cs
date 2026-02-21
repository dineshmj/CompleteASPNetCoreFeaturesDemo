using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MyBank.Common.Business.Validation.Attributes;

namespace MyBank.Common.Entities;

[Table ("Customer")]
public sealed class Customer
	: PersonBase
{
	[Key]
	[DatabaseGenerated (DatabaseGeneratedOption.Identity)]
	[DisplayName ("Customer ID")]
	[DisplayFormat (DataFormatString = "{0:D10}", ApplyFormatInEditMode = true)]
	public int Id { get; set; }
}