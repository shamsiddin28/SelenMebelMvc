using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.DTOs.Orders;

namespace SelenMebel.Service.DTOs.OrderDetails;

public class OrderDetailForResultDto
{
	public long Id { get; set; }
	public long Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public OrderForResultDto Order { get; set; }
	public FurnitureForResultDto Furniture { get; set; }
}
