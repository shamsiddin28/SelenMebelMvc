using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SelenMebel.Domain.Configurations;
using SelenMebel.Domain.Entities;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Helpers;

namespace SelenMebel.Service.Extensions
{
	public static class FurniturePagination
	{
		public static IQueryable<Furniture> ToPagedListFurniture(this IQueryable<Furniture> source, PaginationParams @params)
		{

			var metaData = new PaginationMetaData(source.Count(), @params);

			var json = JsonConvert.SerializeObject(metaData);
			if (HttpContextHelper.ResponseHeaders != null)
			{
				if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
					HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

				HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
			}

			return @params.PageIndex > 0 && @params.PageSize > 0 ?
				source
				.OrderBy(s => s.Id)
				.Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
				: throw new SelenMebelException(400, "Please, enter valid numbers");
		}

		public static IEnumerable<Furniture> ToPagedListFurniture(this IEnumerable<Furniture> source, PaginationParams @params)
		{
			if (@params.PageIndex < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(@params.PageIndex), "The page index must be greater than or equal to 1.");
			}

			if (@params.PageSize < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(@params.PageSize), "The page size must be greater than or equal to 1.");
			}

			return source.Take((@params.PageSize * (@params.PageIndex - 1))..(@params.PageSize * (@params.PageIndex - 1) + @params.PageSize));
		}

		public static IQueryable<FurnitureFeature> ToPagedListFurnitureFeature(this IQueryable<FurnitureFeature> source, PaginationParams @params)
		{

			var metaData = new PaginationMetaData(source.Count(), @params);

			var json = JsonConvert.SerializeObject(metaData);
			if (HttpContextHelper.ResponseHeaders != null)
			{
				if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
					HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

				HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
			}

			return @params.PageIndex > 0 && @params.PageSize > 0 ?
				source
				.OrderBy(s => s.Id)
				.Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
				: throw new SelenMebelException(400, "Please, enter valid numbers");
		}

		public static IEnumerable<FurnitureFeature> ToPagedListFurnitureFeature(this IEnumerable<FurnitureFeature> source, PaginationParams @params)
		{
			if (@params.PageIndex < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(@params.PageIndex), "The page index must be greater than or equal to 1.");
			}

			if (@params.PageSize < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(@params.PageSize), "The page size must be greater than or equal to 1.");
			}

			return source.Take((@params.PageSize * (@params.PageIndex - 1))..(@params.PageSize * (@params.PageIndex - 1) + @params.PageSize));
		}

		public static IQueryable<TypeOfFurniture> ToPagedListTypeOfFurniture(this IQueryable<TypeOfFurniture> source, PaginationParams @params)
		{

			var metaData = new PaginationMetaData(source.Count(), @params);

			var json = JsonConvert.SerializeObject(metaData);
			if (HttpContextHelper.ResponseHeaders != null)
			{
				if (HttpContextHelper.ResponseHeaders.ContainsKey("X-Pagination"))
					HttpContextHelper.ResponseHeaders.Remove("X-Pagination");

				HttpContextHelper.ResponseHeaders.Add("X-Pagination", json);
			}

			return @params.PageIndex > 0 && @params.PageSize > 0 ?
				source
				.OrderBy(s => s.Id)
				.Skip((@params.PageIndex - 1) * @params.PageSize).Take(@params.PageSize)
				: throw new SelenMebelException(400, "Please, enter valid numbers");
		}

		public static IEnumerable<TypeOfFurniture> ToPagedListTypeOfFurniture(this IEnumerable<TypeOfFurniture> source, PaginationParams @params)
		{
			if (@params.PageIndex < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(@params.PageIndex), "The page index must be greater than or equal to 1.");
			}

			if (@params.PageSize < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(@params.PageSize), "The page size must be greater than or equal to 1.");
			}

			return source.Take((@params.PageSize * (@params.PageIndex - 1))..(@params.PageSize * (@params.PageIndex - 1) + @params.PageSize));
		}


	}
}
