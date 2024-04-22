using SelenMebel.Domain.Entities;
using SelenMebel.Service.Commons.Attributes;
using SelenMebel.Service.DTOs.Accounts;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Users
{
	public class UserRegisterDto : AccountRegisterDto
	{
		[Required(ErrorMessage = "Please enter the Email")]
		[Email(ErrorMessage = "The email was entered incorrectly")]
		public string Email { get; set; }

		public static implicit operator User(UserRegisterDto userRegisterDto)
		{
			return new User()
			{
				FirstName = userRegisterDto.FirstName,
				LastName = userRegisterDto.LastName,
				Email = userRegisterDto.Email,
				PhoneNumber = userRegisterDto.PhoneNumber,
				BirthDate = userRegisterDto.BirthDate
			};
		}
	}
}
