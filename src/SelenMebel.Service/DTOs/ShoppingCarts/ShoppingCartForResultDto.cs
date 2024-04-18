using SelenMebel.Service.DTOs.CartDetails;

namespace SelenMebel.Service.DTOs.ShoppingCarts;

public class ShoppingCartForResultDto
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<CartDetailForResultDto> CartDetails { get; set; }
}
