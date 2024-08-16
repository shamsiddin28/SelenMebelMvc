using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Orders;

public class OrderForCreationDto
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public OrderStatus OrderStatus { get; set; }
}
