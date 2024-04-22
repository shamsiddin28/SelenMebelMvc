using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Domain.Configurations;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Extensions;
using SelenMebel.Service.Helpers;
using SelenMebel.Service.Interfaces.TypeOfFurnitures;

namespace SelenMebel.Service.Services.TypeOfFurnitures;

public class TypeOfFurnitureService : ITypeOfFurnitureService
{
	private readonly IMapper _mapper;
	private readonly ICategoryRepository _categoryRepository;
	private readonly IFurnitureRepository _furnitureRepository;
	private readonly ITypeOfFurnitureRepository _typeOfFurnitureRepository;
	private readonly IHostingEnvironment _hostingEnvironment;
	private readonly string TYPEOFFURNITURE_FOLDER;
	private readonly string RESOUCE_IMAGE_FOLDER;


	public TypeOfFurnitureService(
		IMapper mapper,
		ITypeOfFurnitureRepository repository,
		ICategoryRepository categoryRepository,
		IFurnitureRepository furnitureRepository,
		IHostingEnvironment hostingEnvironment)
	{
		_mapper = mapper;
		_typeOfFurnitureRepository = repository;
		_categoryRepository = categoryRepository;
		_furnitureRepository = furnitureRepository;
		TYPEOFFURNITURE_FOLDER = "TypeOfFurnitureImages";
		RESOUCE_IMAGE_FOLDER = Path.Combine(TYPEOFFURNITURE_FOLDER, "Images");
		_hostingEnvironment = hostingEnvironment;
	}

	public async Task<TypeOfFurnitureForResultDto> CreateAsync(TypeOfFurnitureForCreationDto dto)
	{
		//Check is Category
		var category = await _categoryRepository.SelectAll()
				.Where(f => f.Id == dto.CategoryId)
				.FirstOrDefaultAsync();

		if (category is null)
			throw new SelenMebelException(404, "Category is not found !");

		// TypeOfFurniture
		var addTypeOfFurniture = await _typeOfFurnitureRepository.SelectAll()
				.Where(tof => (int)tof.TypeOfSelen == (int)dto.TypeOfSelen && tof.CategoryId == dto.CategoryId)
				.FirstOrDefaultAsync();

		if (addTypeOfFurniture is not null)
			throw new SelenMebelException(409, "TypeOfFurniture already exists");

		var newImagePath = await MediaHelper.UploadFile(dto.Image, _hostingEnvironment.WebRootPath, RESOUCE_IMAGE_FOLDER);

		var mapped = _mapper.Map<TypeOfFurniture>(dto);
		mapped.CreatedAt = DateTime.UtcNow.AddHours(5);
		mapped.Image = newImagePath;

		var result = await _typeOfFurnitureRepository.InsertAsync(mapped);

		return _mapper.Map<TypeOfFurnitureForResultDto>(result);
	}

	public async Task<TypeOfFurnitureForResultDto> ModifyAsync(long id, TypeOfFurnitureForUpdateDto dto)
	{

		// Check is Category
		var category = await _categoryRepository.SelectAll()
				.Where(f => f.Id == dto.CategoryId)
				.FirstOrDefaultAsync();

		if (category is null)
			throw new SelenMebelException(404, "Category is not found !");

		// TypeOfFurniture
		var addTypeOfFurniture = await _typeOfFurnitureRepository.SelectAll()
				.Where(tof => tof.CategoryId == dto.CategoryId)
				.FirstOrDefaultAsync();

		if (addTypeOfFurniture is null)
			throw new SelenMebelException(409, "TypeOfFurniture is not found !");

		var newImagePath = await MediaHelper.UploadFile(dto.Image, _hostingEnvironment.WebRootPath, RESOUCE_IMAGE_FOLDER);

		var imageFullPath = Path.Combine(_hostingEnvironment.WebRootPath, addTypeOfFurniture.Image);

		if (File.Exists(imageFullPath))
			File.Delete(imageFullPath);

		var mappedTypeOfFurniture = _mapper.Map(dto, addTypeOfFurniture);
		mappedTypeOfFurniture.UpdatedAt = DateTime.UtcNow.AddHours(5);
		mappedTypeOfFurniture.Image = newImagePath;

		await _typeOfFurnitureRepository.UpdateAsync(mappedTypeOfFurniture);

		return _mapper.Map<TypeOfFurnitureForResultDto>(mappedTypeOfFurniture);
	}

	public async Task<bool> RemoveAsync(long id)
	{
		var typeOfFurniture = await _typeOfFurnitureRepository.SelectAll()
				.Where(u => u.Id == id)
				.AsNoTracking()
				.FirstOrDefaultAsync();

		if (typeOfFurniture is null)
			throw new SelenMebelException(404, "TypeOfFurniture is not found !");

		var imageFullPath = Path.Combine(_hostingEnvironment.WebRootPath, typeOfFurniture.Image);

		if (File.Exists(imageFullPath))
			File.Delete(imageFullPath);

		var result = await _typeOfFurnitureRepository.DeleteAsync(id);
		typeOfFurniture.IsDeleted = true;

		return result;
	}

	public async Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllAsync(PaginationParams @params)
	{
		var typeOfFurnitures = await _typeOfFurnitureRepository.SelectAll()
				  .Include(c => c.Category)
				  .Include(c => c.Furnitures)
				  .AsNoTracking()
				  .ToPagedList<TypeOfFurniture>(@params)
				  .ToListAsync();

		return _mapper.Map<IEnumerable<TypeOfFurnitureForResultDto>>(typeOfFurnitures);
	}

	public async Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllByPropertiesOfTypeOfFurnituresAsync(string searchTerm)
	{
		var query = _typeOfFurnitureRepository.SelectAll();
		if (!string.IsNullOrEmpty(searchTerm))
		{
			query = query.Where(x => x.Category.Name.ToLower().Contains(searchTerm.ToLower())
								|| x.TypeOfSelen.ToString().Contains(searchTerm.ToLower())
								|| x.Id.ToString().Contains(searchTerm.ToLower()));
		}

		var typeOfFurnitures = await query.OrderByDescending(x => x.CreatedAt)
								  .Include(c => c.Category)
								  .Include(c => c.Furnitures)
								  .AsNoTracking()
								  .ToListAsync();
		return _mapper.Map<IEnumerable<TypeOfFurnitureForResultDto>>(typeOfFurnitures);
	}

	public async Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllTypeOfFurnituresAsync()
	{
		var typeOfFurnitures = await _typeOfFurnitureRepository.SelectAll()
				  .Include(c => c.Category)
				  .Include(c => c.Furnitures)
				  .AsNoTracking()
				  .ToListAsync();

		return _mapper.Map<IEnumerable<TypeOfFurnitureForResultDto>>(typeOfFurnitures);
	}

	public async Task<TypeOfFurnitureForResultDto> RetrieveByIdAsync(long id)
	{
		var typeOfFurniture = await _typeOfFurnitureRepository.SelectAll()
				.Where(u => u.Id == id)
				.Include(c => c.Category)
				.Include(c => c.Furnitures)
				.AsNoTracking()
				.FirstOrDefaultAsync() ??
					throw new SelenMebelException(404, "TypeOfFurniture is not found !");

		return _mapper.Map<TypeOfFurnitureForResultDto>(typeOfFurniture);
	}
}
