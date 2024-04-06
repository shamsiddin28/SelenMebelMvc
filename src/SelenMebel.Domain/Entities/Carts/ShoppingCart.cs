using SelenMebel.Domain.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities.Carts;

[Table("ShoppingCart")]
public class ShoppingCart : Auditable
{
	public long UserId { get; set; }
	public ICollection<CartDetail> CartDetails { get; set; }
}
