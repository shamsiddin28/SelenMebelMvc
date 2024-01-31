using SelenMebel.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelenMebel.Domain.Entities;

[Table("TypeOfFurniture")]
public class TypeOfFurniture
{
    public long Id { get; set; }
    public TypeOfSelen TypeOfSelen { get; set; }
    public string Image { get; set; } = string.Empty;
    
    public long CategoryId { get; set; }    
    public Category Category { get; set; }
}
