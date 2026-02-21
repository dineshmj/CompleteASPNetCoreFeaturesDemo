using System.ComponentModel.DataAnnotations;

namespace MyBank.Common.Business.Validation.Attributes;

public sealed class MustBeAnAdultAttribute
	: ValidationAttribute
{
	private const int MIN_AGE_OF_MAJORITY = 18;
	private const int MAX_AGE_OF_MAJORITY = 22;

	private readonly int _ageOfMajority;

	public MustBeAnAdultAttribute (int ageOfMajority = MIN_AGE_OF_MAJORITY)
	{
		if (
			ageOfMajority < MIN_AGE_OF_MAJORITY
			|| ageOfMajority > MAX_AGE_OF_MAJORITY)
		{
			throw new ArgumentOutOfRangeException (
				nameof (ageOfMajority),
				$"Age of majority must be between {MIN_AGE_OF_MAJORITY} and {MAX_AGE_OF_MAJORITY}.");
		}

		_ageOfMajority = ageOfMajority;
	}

	protected override ValidationResult? IsValid (
		object? value,
		ValidationContext validationContext)
	{
		if (value is not DateTime dateOfBirth)
		{
			return new ValidationResult (
				"The date of birth must be a valid date.",
				new [] { validationContext.MemberName! });
		}

		var minDateOfBirth = DateTime.Today.AddYears (0 - _ageOfMajority);

		if (dateOfBirth < minDateOfBirth)
		{
			return new ValidationResult (
				$"The person must be at least {_ageOfMajority} years old.",
				new [] { validationContext.MemberName! });
		}

		return ValidationResult.Success;
	}
}