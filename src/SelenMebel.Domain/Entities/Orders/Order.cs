using SelenMebel.Domain.Commons;
using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities.Orders;

[Table("Order")]
public class Order : Auditable
{
    public long UserId { get; set; }

    [Required]
    public OrderStatus OrderStatus { get; set; }

    public ICollection<OrderDetail> OrderDetail { get; set; }
}