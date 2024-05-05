using SelenMebel.Domain.Commons;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Service.DTOs.Users
{
    public class UserResultDto : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }

        public static implicit operator UserResultDto(User user)
        {
            return new UserResultDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ImagePath = user.Image!,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate,
                CreatedAt = user.CreatedAt,
            };
        }
    }
}
