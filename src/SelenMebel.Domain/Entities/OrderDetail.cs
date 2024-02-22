using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("OrderDetail")]
public class OrderDetail
{
	public long Id { get; set; }

	[Required]
	public long OrderId { get; set; }
	public Order Order { get; set; }

	[Required]
	public long FurnitureId { get; set; }
	public Furniture Furniture { get; set; }

	[Required]
	public long Quantity { get; set; }

	[Required]
	public decimal UnitPrice { get; set; }

}
