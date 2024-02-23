using SelenMebel.Domain.Enums;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.Furnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebel.Service.DTOs.TypeOfFurnitures;

public class TypeOfFurnitureForResultDto
{
	public long Id { get; set; }
	public TypeOfSelen TypeOfSelen { get; set; }
	public string Image { get; set; }
    
	[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(5);

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? UpdatedAt { get; set; }

    public CategoryForResultDto Category { get; set; }
	public ICollection<FurnitureForResultDto> Furnitures { get; set; }	
}
