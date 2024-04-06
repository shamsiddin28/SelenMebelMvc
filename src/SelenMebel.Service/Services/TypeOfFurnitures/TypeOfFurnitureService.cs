using AutoMapper;
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
    private readonly string ASSETS_FOLDER;
    private readonly string TYPEOFFURNITURE_FOLDER;
    private readonly string RESOUCE_IMAGE_FOLDER;


    public TypeOfFurnitureService(
		IMapper mapper,
		ITypeOfFurnitureRepository repository,
		ICategoryRepository categoryRepository,
		IFurnitureRepository furnitureRepository)
	{
		_mapper = mapper;
		_typeOfFurnitureRepository = repository;
		_categoryRepository = categoryRepository;
		_furnitureRepository = furnitureRepository;
        ASSETS_FOLDER = WebHostEnviromentHelper.WebRootPath;
        TYPEOFFURNITURE_FOLDER = "TypeOfFurnitureImages";
        RESOUCE_IMAGE_FOLDER = Path.Combine(TYPEOFFURNITURE_FOLDER, "Images");
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
			throw new SelenMebelException(409, "TypeOfFurniture alredy exists");

		var image = await MediaHelper.UploadFile(dto.Image, "TypeOfFurnitureImages");

		var mapped = _mapper.Map<TypeOfFurniture>(dto);
		mapped.CreatedAt = DateTime.UtcNow.AddHours(5);
		mapped.Image = image;

		var result = await _typeOfFurnitureRepository.InsertAsync(mapped);

		return _mapper.Map<TypeOfFurnitureForResultDto>(result);
	}

    public async Task<byte[]> DownloadAsync(string imageName)
    {
        var imagePath = Path.Combine(ASSETS_FOLDER, RESOUCE_IMAGE_FOLDER, imageName);
        if (File.Exists(imagePath))
            return await File.ReadAllBytesAsync(imagePath);

        throw new FileNotFoundException();
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

		var image = await MediaHelper.UploadFile(dto.Image, "TypeOfFurnitureImages");

		var mappedTypeOfFurniture = _mapper.Map(dto, addTypeOfFurniture);
		mappedTypeOfFurniture.UpdatedAt = DateTime.UtcNow.AddHours(5);
		mappedTypeOfFurniture.Image = image;

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

		var imageFullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, typeOfFurniture.Image);

		if (File.Exists(imageFullPath))
			File.Delete(imageFullPath);

		await _typeOfFurnitureRepository.DeleteAsync(id);
		typeOfFurniture.IsDeleted = true;

		return true;
	}

	public async Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllAsync(PaginationParams @params)
	{
		var typeOfFurnitures = await _typeOfFurnitureRepository.SelectAll()
				  .Include(c => c.Category)
				  .Include(c => c.Furnitures)
				  .AsNoTracking()
				  .ToPagedListTypeOfFurniture(@params)
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
		var byIdTypeOfFurniture = await _typeOfFurnitureRepository.SelectAll()
				.Where(u => u.Id == id)
				.Include(c => c.Category)
				.Include(c => c.Furnitures)
				.AsNoTracking()
				.FirstOrDefaultAsync() ??
					throw new SelenMebelException(404, "TypeOfFurniture is not found !");

		return _mapper.Map<TypeOfFurnitureForResultDto>(byIdTypeOfFurniture);
	}
}
