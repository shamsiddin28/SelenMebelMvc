using SelenMebel.Data.IRepositories;
using SelenMebel.Data.Repositories;
using SelenMebel.Service.Services.Categories;
using SelenMebel.Service.Services.Furnitures;
using SelenMebel.Service.Interfaces.Categories;
using SelenMebel.Service.Interfaces.Furnitures;
using SelenMebel.Service.Interfaces.FurnitureFeatures;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;
using SelenMebel.Service.Services.FurnitureFeatures;
using SelenMebel.Service.Services.TypeOfFurnitures;

namespace SelenMebel.Api.Extensions;

public static class ServiceExtension
{
	public static void AddCustomService(this IServiceCollection services)
	{
		// Furniture
		services.AddScoped<IFurnitureRepository, FurnitureRepository>();
		services.AddScoped<IFurnitureService, FurnitureService>();

		// TypeOfFurniture
		services.AddScoped<ITypeOfFurnitureRepository, TypeOfFurnitureRepository>();
		services.AddScoped<ITypeOfFurnitureService, TypeOfFurnitureService>();

		// Category
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<ICategoryService, CategoryService>();

		// FurnitureFeature
		services.AddScoped<IFurnitureFeatureRepository, FurnitureFeatureRepository>();
		services.AddScoped<IFurnitureFeatureService, FurnitureFeatureService>();
		

	}
}
