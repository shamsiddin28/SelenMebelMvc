using AutoMapper;
using SelenMebel.Domain.Entities;
using SelenMebel.Domain.Entities.Admins;
using SelenMebel.Domain.Entities.Carts;
using SelenMebel.Domain.Entities.Categories;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Domain.Entities.Orders;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.DTOs.CartDetails;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.FurnitureFeatures;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.DTOs.OrderDetails;
using SelenMebel.Service.DTOs.Orders;
using SelenMebel.Service.DTOs.ShoppingCarts;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using SelenMebel.Service.DTOs.Users;
using SelenMebel.Service.ViewModels.UserViewModels;

namespace SelenMebelMVC.Configuration
{
	public class MappingConfiguration : Profile
	{
		public MappingConfiguration()
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

			CreateMap<AdminRegisterDto, Admin>().ReverseMap();
			CreateMap<UserBaseViewModel, User>().ReverseMap();
			CreateMap<UserViewModel, User>().ReverseMap();
			CreateMap<UserRegisterDto, User>().ReverseMap();
		}
	}
}
