﻿using SelenMebel.Service.Commons.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Admins;

public class PasswordUpdateDto
{
	[Required(ErrorMessage = "Enter a password!")]
	[StrongPassword]
	public string OldPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "Enter a password!")]
	[StrongPassword]
	public string NewPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "Enter a password!")]
	[StrongPassword]
	public string VerifyPassword { get; set; } = string.Empty;
}
