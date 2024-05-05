using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Data.Repositories;
using SelenMebel.Data.Repositories.Commons;
using SelenMebel.Service.Interfaces.Accounts;
using SelenMebel.Service.Interfaces.Admins;
using SelenMebel.Service.Interfaces.Categories;
using SelenMebel.Service.Interfaces.Commons;
using SelenMebel.Service.Interfaces.Files;
using SelenMebel.Service.Interfaces.FurnitureFeatures;
using SelenMebel.Service.Interfaces.Furnitures;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;
using SelenMebel.Service.Interfaces.Users;
using SelenMebel.Service.Mappers;
using SelenMebel.Service.Services.Accounts;
using SelenMebel.Service.Services.Admins;
using SelenMebel.Service.Services.Categories;
using SelenMebel.Service.Services.Commons;
using SelenMebel.Service.Services.Files;
using SelenMebel.Service.Services.FurnitureFeatures;
using SelenMebel.Service.Services.Furnitures;
using SelenMebel.Service.Services.TypeOfFurnitures;
using SelenMebel.Service.Services.Users;

namespace SelenMebelMVC.Configuration.LayerConfigurations
{
    public static class ServiceLayerConfiguration
    {
        public static void AddService(this IServiceCollection services)
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

            // Admins 
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminService, AdminService>();

            // Users
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IFileService, FileService>();


            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperProfile));
        }
    }
}
