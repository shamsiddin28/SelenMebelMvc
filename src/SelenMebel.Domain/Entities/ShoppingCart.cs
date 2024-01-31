using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("ShoppingCart")]
public class ShoppingCart
{
    public long Id { get; set; }

    [Required]
    public string UserId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<CartDetail> CartDetails { get; set; }
}
