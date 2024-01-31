namespace SelenMebelMvcUI.Models.DTOs
{
    public class FurnitureViewModel
    {
        public IEnumerable<Furniture> Furnitures { get; set; } 
        public IEnumerable<Category> Categories { get; set; }
        public string Sterm { get; set; } = "";
        public long UniqueId { get; set; } = 0;
        public long CategoryId { get; set; } = 0;
    }
}
