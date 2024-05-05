using SelenMebel.Service.DTOs.TypeOfFurnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.Categories;

public class CategoryForResultDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(5);

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? UpdatedAt { get; set; }

    public ICollection<TypeOfFurnitureForResultDto> TypeOfFurnitures { get; set; }
}
