namespace SelenMebel.Service.ViewModels
{
    public class AccountBaseViewModel
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;
    }
}
