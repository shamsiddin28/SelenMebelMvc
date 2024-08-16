using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.ShoppingCarts;

namespace SelenMebel.Service.Interfaces.ShoppingCarts;

public interface IShoppingCartService
{
    Task<bool> RemoveAsync(long id);
    Task<ShoppingCartForResultDto> RetrieveByIdAsync(long id);
    Task<ShoppingCartForResultDto> CreateAsync(ShoppingCartForCreationDto dto);
    Task<IEnumerable<ShoppingCartForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
