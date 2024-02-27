using AutoMapper;
using SelenMebel.Domain.Entities;
using SelenMebel.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using SelenMebel.Service.Exceptions;
using SelenMebel.Data.IRepositories;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.Interfaces.Categories;

namespace SelenMebel.Service.Services.Categories;

public class CategoryService : ICategoryService
{
	private readonly IMapper _mapper;
	private readonly ICategoryRepository _repository;

	public CategoryService(IMapper mapper, ICategoryRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}


	public async Task<CategoryForResultDto> CreateAsync(CategoryForCreationDto dto)
	{
		var category = await _repository.SelectAll()
			.Where(c => c.Name.ToLower() == dto.Name.ToLower())
			.AsNoTracking()
			.FirstOrDefaultAsync();

		if (category is not null)
			throw new SelenMebelException(409, "Category is already exist !");

		var image = await MediaHelper.UploadFile(dto.Image, "CategoryImages");

		var mappedCategory = _mapper.Map<Category>(dto);
		mappedCategory.CreatedAt = DateTime.UtcNow.AddHours(5);
		mappedCategory.Image = image;

		var result = await _repository.InsertAsync(mappedCategory);

		return this._mapper.Map<CategoryForResultDto>(result);
	}

	public async Task<CategoryForResultDto> ModifyAsync(long id, CategoryForUpdateDto dto)
	{
		var category = await _repository.SelectAll()
				.Where(ud => ud.Id == id)
				.AsNoTracking()
				.FirstOrDefaultAsync();

		if (category is null)
			throw new SelenMebelException(404, "Category is not found !");

		var image = await MediaHelper.UploadFile(dto.Image, "CategoryImages");
		var mappedCategory = this._mapper.Map(dto, category);
		mappedCategory.UpdatedAt = DateTime.UtcNow.AddHours(5);
		mappedCategory.Image = image;

		var result = await this._repository.UpdateAsync(mappedCategory);

		return this._mapper.Map<CategoryForResultDto>(result);
	}

	public async Task<bool> RemoveAsync(long id)
	{
		var category = await _repository.SelectAll()
			  .Where(c => c.Id == id)
			  .AsNoTracking()
			  .FirstOrDefaultAsync();

		if (category is null)
			throw new SelenMebelException(404, "Category is not found !");

		var imageFullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, category.Image);

		if (File.Exists(imageFullPath))
			File.Delete(imageFullPath);

		var result = await _repository.DeleteAsync(id);
		category.IsDeleted = true;

		return result;
	}

	public async Task<IEnumerable<CategoryForResultDto>> RetrieveAllAsync()
	{
		var category = await _repository.SelectAll()
			.Include(tof => tof.TypeOfFurnitures)
			.ThenInclude(f => f.Furnitures)
			.AsNoTracking()
			.ToListAsync();


		return _mapper.Map<IEnumerable<CategoryForResultDto>>(category);
	}

	public async Task<CategoryForResultDto> RetrieveByIdAsync(long id)
	{
		var category = await this._repository.SelectAll()
				.Where(c => c.Id == id)
				.Include(tof => tof.TypeOfFurnitures)
				.ThenInclude(f => f.Furnitures)
                .AsNoTracking()
				.FirstOrDefaultAsync();

		if (category is null)
			throw new SelenMebelException(404, "Category is not found !");

		return this._mapper.Map<CategoryForResultDto>(category);
	}
}
