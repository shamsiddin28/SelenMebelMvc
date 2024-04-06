using SelenMebel.Domain.Entities;

namespace SelenMebel.Service.ViewModels.UserViewModels;

public class UserViewModel
{
    public long Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    
    public string ImagePath { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }

    public DateTime CreatedAt { get; set; } = default!;

    public static implicit operator UserViewModel(User model)
    {
        return new UserViewModel
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            ImagePath = model.Image!,
            PhoneNumber = model.PhoneNumber,
            BirthDate = model.BirthDate,
            CreatedAt = model.CreatedAt
        };
    }
}
