namespace SelenMebel.Domain.Entities;

public class User : Human
{
    public string Email { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}
