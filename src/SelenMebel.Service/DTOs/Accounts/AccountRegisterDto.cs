using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Accounts;

public class AccountRegisterDto : AccountLoginDto
{
    [Required(ErrorMessage = "Enter a name!")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Enter a surname!")]
    public string LastName { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}
