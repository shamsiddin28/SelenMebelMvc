using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.Orders;

namespace SelenMebel.Service.Interfaces.Orders;

public interface IOrderService
{
	Task<bool> RemoveAsync(long id);
	Task<OrderForResultDto> RetrieveByIdAsync(long id);
	Task<OrderForResultDto> CreateAsync(OrderForCreationDto dto);
	Task<IEnumerable<OrderForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
