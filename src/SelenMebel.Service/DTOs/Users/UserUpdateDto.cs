using Microsoft.AspNetCore.Http;
using SelenMebel.Domain.Entities;
using SelenMebel.Service.Commons.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Users
{
	public class UserUpdateDto
	{
		public long Id { get; set; }

		[Required(ErrorMessage = "Please enter the Firstname!")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter the LastName!")]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter the Email!")]
		[Email(ErrorMessage = "The email was entered incorrectly")]
		public string Email { get; set; }

		[AllowedFiles(new string[] { ".jpg", ".png", ".jpeg", ".svg", ".webp" }), MaxFile(2)]
		public IFormFile Image { get; set; }
		public string ImagePath { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter the Phone number!")]
		public string PhoneNumber { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter the Birth date!")]
		public DateTime BirthDate { get; set; }

		public static implicit operator User(UserUpdateDto dto)
		{
			return new User()
			{
				Id = dto.Id,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Email = dto.Email,
				Image = dto.ImagePath,
				PhoneNumber = dto.PhoneNumber,
				BirthDate = dto.BirthDate,
			};
		}
	}
}
