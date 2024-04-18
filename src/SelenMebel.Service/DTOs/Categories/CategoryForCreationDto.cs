﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Categories;

public class CategoryForCreationDto
{
    [Required, MaxLength(40)]
    public string Name { get; set; }

    [Required]
    public IFormFile Image { get; set; }
}
