using SelenMebel.Domain.Entities;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.DTOs.ShoppingCarts;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.CartDetails;

public class CartDetailForResultDto
{
	public long Id { get; set; }
	public long Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public FurnitureForResultDto Furniture { get; set; }
	public ShoppingCartForResultDto ShoppingCart { get; set; }
}
