using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.Commons.Attributes;

public class EmailAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is null)
		{
			return new ValidationResult("Email address is required!");
		}
		else
		{
			string email = value.ToString()!;

			if (!IsValidEmail(email))
			{
				return new ValidationResult("Invalid email address format.");
			}

			return ValidationResult.Success;
		}
	}

	// Method to validate the email address using regular expression
	private bool IsValidEmail(string email)
	{
		// Define a regular expression pattern for valid email addresses
		// This pattern is a basic validation and may not cover all cases
		string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

		// Use Regex.IsMatch method to check if the email matches the pattern
		return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
	}
}
