using SelenMebel.Domain.Enums;
using SelenMebel.Service.DTOs.OrderDetails;

namespace SelenMebel.Service.DTOs.Orders;

public class OrderForResultDto
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<OrderDetailForResultDto> OrderDetail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
