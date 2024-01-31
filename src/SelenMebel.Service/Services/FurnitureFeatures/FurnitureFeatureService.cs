namespace SelenMebel.Service.Services.FurnitureFeatures;

public class FurnitureFeatureService
{

    //private readonly IMapper _mapper;
    //private readonly IRepository<FurnitureFeature> _repository;

    //public FurnitureFeatureService(IMapper mapper, IRepository<FurnitureFeature> repository)
    //{
    //    _mapper = mapper;
    //    _repository = repository;
    //}

    //public async Task<FurnitureFeatureForResultDto> CreateAsync(FurnitureFeatureForCreationDto dto)
    //{
    //    var addFurnitureFeature = await _repository.SelectAll()
    //            .Where(tof => tof.Name.ToLower() == dto.Name.ToLower())
    //            .FirstOrDefaultAsync();

    //    if (addFurnitureFeature is not null)
    //        throw new SelenMebelException(409, "Furniture already exists");

    //    var mapped = _mapper.Map<FurnitureFeature>(dto);
    //    mapped.CreatedAt = DateTime.UtcNow;

    //    var result = await _repository.InsertAsync(mapped);

    //    return _mapper.Map<FurnitureFeatureForResultDto>(result);
    //}

    //public async Task<FurnitureFeatureForResultDto> ModifyAsync(long id, FurnitureFeatureForUpdateDto dto)
    //{
    //    var furniture = await _repository.SelectAll()
    //           .Where(u => u.Id == id)
    //           .AsNoTracking()
    //           .FirstOrDefaultAsync();

    //    if (furniture is null)
    //        throw new SelenMebelException(404, "FurnitureFeature is not found");

    //    var mappedFurnitureFeature = this._mapper.Map(dto, furniture);

    //    mappedFurnitureFeature.UpdatedAt = DateTime.UtcNow;

    //    await this._repository.UpdateAsync(mappedFurnitureFeature);

    //    return this._mapper.Map<FurnitureFeatureForResultDto>(mappedFurnitureFeature);
    //}

    //public async Task<bool> RemoveAsync(long id)
    //{
    //    var furnitureFeature = await _repository.SelectAll()
    //            .Where(u => u.Id == id)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync();

    //    if (furnitureFeature is null)
    //        throw new SelenMebelException(404, "FurnitureFeature is not found !");

    //    await _repository.DeleteAsync(id);
    //    furnitureFeature.IsDeleted = true;

    //    return true;
    //}

    //public async Task<IEnumerable<FurnitureFeatureForResultDto>> RetrieveAllAsync(PaginationParams @params)
    //{
    //    var furnitureFeatures = await _repository.SelectAll()
    //              .Include(ff => ff.FurnitureFeatures)
    //              .AsNoTracking()
    //              .ToPagedList(@params)
    //              .ToListAsync();

    //    return _mapper.Map<IEnumerable<FurnitureFeatureForResultDto>>(furnitureFeatures);
    //}

    //public async Task<FurnitureFeatureForResultDto> RetrieveByIdAsync(long id)
    //{
    //    var byFurnitureFeature = await _repository.SelectAll()
    //            .Where(u => u.Id == id)
    //            .Include(f => f.FurnitureFeatures)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync() ??
    //                throw new SelenMebelException(404, "FurnitureFeature is not found! ");

    //    return _mapper.Map<FurnitureFeatureForResultDto>(byFurnitureFeature);
    //}

}
