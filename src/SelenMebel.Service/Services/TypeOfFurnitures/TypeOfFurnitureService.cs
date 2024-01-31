namespace SelenMebel.Service.Services.TypeOfFurnitures;

public class TypeOfFurnitureService
{
    //private readonly IMapper _mapper;
    //private readonly IRepository<TypeOfFurniture> _repository;

    //public TypeOfFurnitureService(IRepository<TypeOfFurniture> repository, IMapper mapper)
    //{
    //    _repository = repository;
    //    _mapper = mapper;
    //}

    //public async Task<TypeOfFurnitureForResultDto> CreateAsync(TypeOfFurnitureForCreationDto dto)
    //{
    //    var addTypeOfFurniture = await _repository.SelectAll()
    //            .Where(tof => tof.Name.ToLower() == dto.Name.ToLower())
    //            .FirstOrDefaultAsync();

    //    if (addTypeOfFurniture is not null)
    //        throw new SelenMebelException(409, "TypeOfFurniture alredy exists");

    //    var image = await MediaHelper.UploadFile(dto.Image, "TypeOfFurnitureImages");

    //    var mapped = _mapper.Map<TypeOfFurniture>(dto);
    //    mapped.CreatedAt = DateTime.UtcNow;
    //    mapped.Image = image;

    //    var result = await _repository.InsertAsync(mapped);

    //    return _mapper.Map<TypeOfFurnitureForResultDto>(result);
    //}

    //public async Task<TypeOfFurnitureForResultDto> ModifyAsync(long id, TypeOfFurnitureForUpdateDto dto)
    //{
    //    var typeOfFurniture = await _repository.SelectAll()
    //           .Where(u => u.Id == id)
    //           .AsNoTracking()
    //           .FirstOrDefaultAsync();

    //    if (typeOfFurniture is null)
    //        throw new SelenMebelException(404, "TypeOfFurniture is not found");

    //    var image = await MediaHelper.UploadFile(dto.Image, "TypeOfFurnitureImages");    

    //    var mappedTypeOfFurniture = this._mapper.Map(dto, typeOfFurniture);
    //    mappedTypeOfFurniture.UpdatedAt = DateTime.UtcNow;
    //    mappedTypeOfFurniture.Image = image;

    //    await this._repository.UpdateAsync(mappedTypeOfFurniture);

    //    return this._mapper.Map<TypeOfFurnitureForResultDto>(mappedTypeOfFurniture);
    //}

    //public async Task<bool> RemoveAsync(long id)
    //{
    //    var typeOfFurniture = await _repository.SelectAll()
    //            .Where(u => u.Id == id)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync();

    //    if (typeOfFurniture is null)
    //        throw new SelenMebelException(404, "TypeOfFurniture is not found !");

    //    var imageFullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, typeOfFurniture.Image);

    //    if (File.Exists(imageFullPath))
    //        File.Delete(imageFullPath);

    //    await _repository.DeleteAsync(id);
    //    typeOfFurniture.IsDeleted = true;

    //    return true;
    //}

    //public async Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllAsync(PaginationParams @params)
    //{
    //    var typeOfFurnitures = await _repository.SelectAll()
    //              .Include(tof => tof.Furniture)
    //              .ThenInclude(ff => ff.FurnitureFeature)
    //              .AsNoTracking()
    //              .ToPagedList(@params)
    //              .ToListAsync();

    //    return _mapper.Map<IEnumerable<TypeOfFurnitureForResultDto>>(typeOfFurnitures);
    //}

    //public async Task<TypeOfFurnitureForResultDto> RetrieveByIdAsync(long id)
    //{
    //    var byIdTypeOfFurniture = await _repository.SelectAll()
    //            .Where(u => u.Id == id)
    //            .Include(tof => tof.Furniture)
    //            .ThenInclude(ff => ff.FurnitureFeature)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync() ??
    //                throw new SelenMebelException(404, "TypeOfFurniture is not found! ");

    //    return _mapper.Map<TypeOfFurnitureForResultDto>(byIdTypeOfFurniture);
    //}
}
