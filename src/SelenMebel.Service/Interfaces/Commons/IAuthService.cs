using SelenMebel.Domain.Entities;

namespace SelenMebel.Service.Interfaces.Commons;

public interface IAuthService
{
    string GenerateToken(Human human, string role);
}
