using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("Order")]
public class Order
{
    public long Id { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    public OrderStatus OrderStatus { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderDetail> OrderDetail { get; set; }
}