using AutoMapper;
using SelenMebel.Domain.Entities;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.FurnitureCategories;
using SelenMebel.Service.DTOs.FurnitureFeature;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.DTOs.TypeOfFurniture;

namespace SelenMebel.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Category
        CreateMap<Category, CategoryForCreationDto>().ReverseMap();
        CreateMap<Category, CategoryForUpdateDto>().ReverseMap();
        CreateMap<Category, CategoryForResultDto>().ReverseMap();

        // Furniture
        CreateMap<Furniture, FurnitureForCreationDto>().ReverseMap();
        CreateMap<Furniture, FurnitureForUpdateDto>().ReverseMap();
        CreateMap<Furniture, FurnitureForResultDto>().ReverseMap();

        // FurnitureFeature
        CreateMap<FurnitureFeature, FurnitureFeatureForCreationDto>().ReverseMap();
        CreateMap<FurnitureFeature, FurnitureFeatureForUpdateDto>().ReverseMap();
        CreateMap<FurnitureFeature, FurnitureFeatureForResultDto>().ReverseMap();

        // TypeOfFurniture
        CreateMap<TypeOfFurniture, TypeOfFurnitureForCreationDto>().ReverseMap();
        CreateMap<TypeOfFurniture, TypeOfFurnitureForUpdateDto>().ReverseMap();
        CreateMap<TypeOfFurniture, TypeOfFurnitureForResultDto>().ReverseMap();





    }
}
