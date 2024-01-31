using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("CartDetail")]
public class CartDetail
{
    public long Id { get; set; }

    [Required]
    public long ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; }

    [Required]
    public long FurnitureId { get; set; }
    public Furniture Furniture { get; set; }

    public long Quantity { get; set; }

}
