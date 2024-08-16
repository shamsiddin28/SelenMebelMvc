using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Domain.Enums;

public enum OrderStatus
{
    [Display(Name = "Pending")]
    Pending = 1,

    [Display(Name = "Processing")]
    Processing = 2,

    [Display(Name = "Shipped")]
    Shipped = 3,

    [Display(Name = "Cancelled")]
    Cancelled = 4
}
