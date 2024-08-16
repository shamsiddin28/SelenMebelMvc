using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.CartDetails;

namespace SelenMebel.Service.Interfaces.CartDetails;

public interface ICartDetailService
{
    Task<bool> RemoveAsync(long id);
    Task<CartDetailForResultDto> RetrieveByIdAsync(long id);
    Task<CartDetailForResultDto> CreateAsync(CartDetailForCreationDto dto);
    Task<CartDetailForResultDto> ModifyAsync(long id, CartDetailForUpdateDto dto);
    Task<IEnumerable<CartDetailForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
