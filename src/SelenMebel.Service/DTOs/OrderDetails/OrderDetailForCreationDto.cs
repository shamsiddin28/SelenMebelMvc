using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.OrderDetails;

public class OrderDetailForCreationDto
{
    [Required]
    public long OrderId { get; set; }

    [Required]
    public long FurnitureId { get; set; }

    [Required]
    public long Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }
}
