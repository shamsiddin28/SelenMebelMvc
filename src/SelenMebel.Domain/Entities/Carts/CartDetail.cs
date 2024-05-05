using SelenMebel.Domain.Commons;
using SelenMebel.Domain.Entities.Furnitures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities.Carts;

[Table("CartDetail")]
public class CartDetail : Auditable
{
    [Required]
    public long ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; }

    [Required]
    public long FurnitureId { get; set; }
    public Furniture Furniture { get; set; }

    public long Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
