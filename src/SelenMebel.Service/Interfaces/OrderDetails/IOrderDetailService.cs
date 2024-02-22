using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.DTOs.OrderDetails;
using SelenMebel.Service.DTOs.Orders;

namespace SelenMebel.Service.Interfaces.OrderDetails;

public interface IOrderDetailService
{
	Task<bool> RemoveAsync(long id);
	Task<OrderDetailForResultDto> RetrieveByIdAsync(long id);
	Task<OrderDetailForResultDto> CreateAsync(OrderDetailForCreationDto dto);
	Task<IEnumerable<OrderDetailForResultDto>> RetrieveAllAsync(PaginationParams @params);
}
