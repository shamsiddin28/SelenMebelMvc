using Microsoft.AspNetCore.Mvc.Rendering;
using SelenMebel.Domain.Entities;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.Furnitures;
using System.ComponentModel.DataAnnotations;

namespace SelenMebelMVC.Models
{
	public class FurnitureViewModel
	{
		public long CategoryId { get; set; }
		public long TypeOfId { get; set; }
		public string TypeOfImage { get; set; }
		public string SelectedCountry { get; set; }

		public List<SelectListItem> CategoriesSelectList { get; set; }
		public List<SelectListItem> EnumsSelectList { get; set; }

		public FurnitureForCreationDto Furniture { get; set; }

		public IEnumerable<Category> Categories { get; set; }
    }
}
