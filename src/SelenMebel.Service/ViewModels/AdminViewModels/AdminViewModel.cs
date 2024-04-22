using SelenMebel.Domain.Entities.Admins;
using SelenMebel.Domain.Enums;

namespace SelenMebel.Service.ViewModels.AdminViewModels
{
	public class AdminViewModel
	{
		public long Id { get; set; }

		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public string ImagePath { get; set; } = string.Empty;

		public string PhoneNumber { get; set; } = string.Empty;

		public DateTime BirthDate { get; set; } = default!;

		public string Address { get; set; } = string.Empty;

		public Role Role { get; set; }

		public DateTime CreatedAt { get; set; } = default!;

		public DateTime? UpdatedAt { get; set; }

		public static implicit operator AdminViewModel(Admin model)
		{
			return new AdminViewModel()
			{
				Id = model.Id,
				FirstName = model.FirstName,
				LastName = model.LastName,
				ImagePath = model.Image!,
				Role = model.AdminRole,
				PhoneNumber = model.PhoneNumber,
				BirthDate = model.BirthDate,
				Address = model.Address,
				CreatedAt = model.CreatedAt,
				UpdatedAt = model.UpdatedAt
			};
		}
	}
}
