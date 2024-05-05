using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Domain.Enums;

public enum Role
{
    [Display(Name = "SuperAdmin")]
    SuperAdmin = 1,

    [Display(Name = "Admin")]
    Admin = 2
}
