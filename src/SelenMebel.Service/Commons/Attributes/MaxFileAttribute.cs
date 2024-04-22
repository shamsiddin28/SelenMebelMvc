﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.Commons.Attributes;

public class MaxFileAttribute : ValidationAttribute
{
	private readonly int _maxFileSize;
	public MaxFileAttribute(int maxFileSize)
	{
		_maxFileSize = maxFileSize * 1024 * 1024;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is IFormFile file)
		{
			if (file.Length > _maxFileSize)
			{
				return new ValidationResult(GetErrorMessage());
			}
		}
		return ValidationResult.Success!;
	}

	public string GetErrorMessage()
	{
		return $"Maximum allowed file size is {_maxFileSize} bytes.";
	}
}
