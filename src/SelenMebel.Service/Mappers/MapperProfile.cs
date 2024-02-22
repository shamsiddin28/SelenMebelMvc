using AutoMapper;
using SelenMebel.Domain.Entities;
using SelenMebel.Service.DTOs.Orders;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.DTOs.CartDetails;
using SelenMebel.Service.DTOs.OrderDetails;
using SelenMebel.Service.DTOs.ShoppingCarts;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using SelenMebel.Service.DTOs.FurnitureFeatures;

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

		// Order
		CreateMap<Order, OrderForCreationDto>().ReverseMap();
		CreateMap<Order, OrderForResultDto>().ReverseMap();
		CreateMap<Order, OrderForUpdateDto>().ReverseMap();

		// OrderDetail
		CreateMap<OrderDetail, OrderDetailForCreationDto>().ReverseMap();
		CreateMap<OrderDetail, OrderDetailForResultDto>().ReverseMap();
		CreateMap<OrderDetail, OrderDetailForUpdateDto>().ReverseMap();

		// ShoppingCart
		CreateMap<ShoppingCart, ShoppingCartForCreationDto>().ReverseMap();
		CreateMap<ShoppingCart, ShoppingCartForUpdateDto>().ReverseMap();
		CreateMap<ShoppingCart, ShoppingCartForResultDto>().ReverseMap();

		// CartDetail
		CreateMap<CartDetail, CartDetailForCreationDto>().ReverseMap();
		CreateMap<CartDetail, CartDetailForResultDto>().ReverseMap();
		CreateMap<CartDetail, CartDetailForUpdateDto>().ReverseMap();
	}
}
