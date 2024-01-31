using SelenMebel.Domain.Entities;
using SelenMebel.Service.Interfaces.Categories;
using SelenMebel.Service.Interfaces.FurnitureCategories;
using SelenMebel.Service.Interfaces.FurnitureFeatures;
using SelenMebel.Service.Interfaces.Furnitures;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;
using SelenMebel.Service.Services.Categories;
using SelenMebel.Service.Services.FurnitureCategories;
using SelenMebel.Service.Services.FurnitureFeatures;
using SelenMebel.Service.Services.Furnitures;
using SelenMebel.Service.Services.TypeOfFurnitures;

namespace SelenMebel.Api.Extensions;

public static class ServiceExtension
{
    public static void AddCustomService(this IServiceCollection services)
    {
        // Furniture
        //services.AddScoped<IFurnitureService, FurnitureService>();
        //services.AddScoped<IRepository<Furniture>, Repository<Furniture>>();

        //// TypeOfFurniture
        //services.AddScoped<ITypeOfFurnitureService, TypeOfFurnitureService>();
        //services.AddScoped<IRepository<TypeOfFurniture>, Repository<TypeOfFurniture>>();

        //// Category
        //services.AddScoped<ICategoryService, CategoryService>();
        //services.AddScoped<IRepository<Category>, Repository<Category>>();

        //// FurnitureFeature
        //services.AddScoped<IFurnitureFeatureService, FurnitureFeatureService>();
        //services.AddScoped<IRepository<FurnitureFeature>, Repository<FurnitureFeature>>();

        //// FurnitureCategory
        //services.AddScoped<IFurnitureCategoryService, FurnitureCategoryService>();
        //services.AddScoped<IRepository<FurnitureCategory>, Repository<FurnitureCategory>>();

    }
}
