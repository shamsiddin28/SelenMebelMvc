using SelenMebel.Domain.Enums;

namespace SelenMebel.Domain.Entities.Admins;

public class Admin : Human
{
    public string Address { get; set; } = string.Empty;

    public Role AdminRole { get; set; } = Role.Admin;

    public string PasswordHash { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;
}
