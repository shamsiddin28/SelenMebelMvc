namespace SelenMebel.Service.Services.FurnitureCategories;

public class FurnitureCategoryService
{
    //private readonly IMapper _mapper;
    //private readonly IRepository<FurnitureCategory> _repository;
    //private readonly IRepository<Furniture> _furnitureRepository;
    //private readonly IRepository<Category> _categoryRepository;

    //public FurnitureCategoryService(
    //    IMapper mapper,
    //    IRepository<FurnitureCategory> repository,
    //    IRepository<Furniture> furnitureRepository,
    //    IRepository<Category> categoryRepository
    //    )
    //{
    //    _mapper = mapper;
    //    _repository = repository;
    //    _furnitureRepository = furnitureRepository;
    //    _categoryRepository = categoryRepository;
    //}

    //public async Task<FurnitureCategoryForResultDto> CreateAsync(FurnitureCategoryForCreationDto dto)
    //{
    //    var furniture = _furnitureRepository.SelectAll()
    //        .Where(f => f.Id == dto.FurnitureId)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();
    //    if (furniture is null)
    //        throw new SelenMebelException(404, "Furniture is not found");

    //    var category = _categoryRepository.SelectAll()
    //        .Where(c => c.Id == dto.CategoryId)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();
    //    if (category is null)
    //        throw new SelenMebelException(404, "Category is not found");

    //    var mapped = _mapper.Map<FurnitureCategory>(dto);
    //    mapped.CreatedAt = DateTime.UtcNow;

    //    var result = await _repository.InsertAsync(mapped);

    //    return _mapper.Map<FurnitureCategoryForResultDto>(result);
    //}

    //public async Task<FurnitureCategoryForResultDto> ModifyAsync(long id, FurnitureCategoryForUpdateDto dto)
    //{
    //    var furniture = _furnitureRepository.SelectAll()
    //        .Where(f => f.Id == dto.FurnitureId)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();
    //    if (furniture is null)
    //        throw new SelenMebelException(404, "Furniture is not found");

    //    var category = _categoryRepository.SelectAll()
    //        .Where(c => c.Id == dto.CategoryId)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();
    //    if (category is null)
    //        throw new SelenMebelException(404, "Category is not found");

    //    var furnitureCategory = await _repository.SelectAll()
    //        .Where(uc => uc.Id == id)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();
    //    if (furnitureCategory is null)
    //        throw new SelenMebelException(404, "FurnitureCategory is not found");

    //    var mappedFurnitureCategory = _mapper.Map(dto, furnitureCategory);
    //    mappedFurnitureCategory.UpdatedAt = DateTime.UtcNow;

    //    await _repository.UpdateAsync(mappedFurnitureCategory);

    //    return _mapper.Map<FurnitureCategoryForResultDto>(mappedFurnitureCategory);
    //}

    //public async Task<bool> RemoveAsync(long id)
    //{
    //    var furnitureCategory = await _repository.SelectAll()
    //        .Where(fc => fc.Id == id)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();
    //    if (furnitureCategory is null)
    //        throw new SelenMebelException(404, "FurnitureCategory is not found");

    //    return await _repository.DeleteAsync(id);
    //}

    //public async Task<IEnumerable<FurnitureCategoryForResultDto>> RetrieveAllAsync(PaginationParams @params)
    //{
    //    var furnitureCategory = _repository.SelectAll()
    //        .Include(c => c.Category)
    //        .ThenInclude(tof => tof.TypeOfFurniture)
    //        .ThenInclude(f => f.Furniture)
    //        .ThenInclude(ff => ff.FurnitureFeature)
    //        .ToPagedList(@params);

    //    return this._mapper.Map<IEnumerable<FurnitureCategoryForResultDto>>(furnitureCategory);
    //}

    //public async Task<FurnitureCategoryForResultDto> RetrieveByIdAsync(long id)
    //{
    //    var data = await this._repository
    //        .SelectAll()
    //        .Where(fc => fc.Id == id)
    //        .Include(c => c.Category)
    //        .ThenInclude(c => c.TypeOfFurniture)
    //        .ThenInclude(f => f.Furniture)
    //        .ThenInclude(ff => ff.FurnitureFeature)
    //        .AsNoTracking()
    //        .FirstOrDefaultAsync();

    //    if (data is null)
    //        throw new SelenMebelException(404, "FurnitureCategory is not found");

    //    return this._mapper.Map<FurnitureCategoryForResultDto>(data);
    //}
}
