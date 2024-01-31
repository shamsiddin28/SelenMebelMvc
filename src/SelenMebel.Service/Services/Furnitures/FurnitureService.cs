namespace SelenMebel.Service.Services.Furnitures;

public class FurnitureService
{
    //private readonly IMapper _mapper;
    //private readonly IRepository<Furniture> _repository;

    //public FurnitureService(IRepository<Furniture> repository, IMapper mapper)
    //{
    //    _repository = repository;
    //    _mapper = mapper;
    //}

    //public async Task<FurnitureForResultDto> CreateAsync(FurnitureForCreationDto dto)
    //{
    //    var addFurniture = await _repository.SelectAll()
    //            .Where(tof => tof.Name.ToLower() == dto.Name.ToLower())
    //            .FirstOrDefaultAsync();

    //    if (addFurniture is not null)
    //        throw new SelenMebelException(409, "Furniture alredy exists");

    //    var image = await MediaHelper.UploadFile(dto.Image, "FurnitureImages");

    //    var mapped = _mapper.Map<Furniture>(dto);

    //    mapped.UniqueId = long.Parse(GenerateOtpDigits());
    //    mapped.CreatedAt = DateTime.UtcNow;
    //    mapped.Image = image;

    //    var result = await _repository.InsertAsync(mapped);

    //    return _mapper.Map<FurnitureForResultDto>(result);
    //}

    //public string GenerateOtpDigits()
    //{
    //    string num = "0123456789";
    //    int len = num.Length;
    //    string otp = string.Empty;

    //    int otpDigit = 5;
    //    string finalDigit;
    //    int getIndex;
    //    for (int i = 0; i < otpDigit; i++)
    //    {
    //        do
    //        {
    //            getIndex = new Random().Next(0, len);
    //            finalDigit = num.ToCharArray()[getIndex].ToString();
    //        } while (otp.IndexOf(finalDigit) != -1);
    //        otp += finalDigit;
    //    }
    //    return otp;
    //}

    //public async Task<FurnitureForResultDto> ModifyAsync(long id, FurnitureForUpdateDto dto)
    //{
    //    var furniture = await _repository.SelectAll()
    //           .Where(u => u.Id == id)
    //           .AsNoTracking()
    //           .FirstOrDefaultAsync();

    //    if (furniture is null)
    //        throw new SelenMebelException(404, "Furniture is not found");

    //    var image = await MediaHelper.UploadFile(dto.Image, "FurnitureImages");

    //    var mappedFurniture = this._mapper.Map(dto, furniture);

    //    mappedFurniture.UpdatedAt = DateTime.UtcNow;
    //    mappedFurniture.Image = image;
    //    mappedFurniture.UniqueId = long.Parse(GenerateOtpDigits());

    //    await this._repository.UpdateAsync(mappedFurniture);

    //    return this._mapper.Map<FurnitureForResultDto>(mappedFurniture);
    //}

    //public async Task<bool> RemoveAsync(long id)
    //{
    //    var furniture = await _repository.SelectAll()
    //            .Where(u => u.Id == id)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync();

    //    if (furniture is null)
    //        throw new SelenMebelException(404, "Furniture is not found !");

    //    var imageFullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, furniture.Image);

    //    if (File.Exists(imageFullPath))
    //        File.Delete(imageFullPath);

    //    await _repository.DeleteAsync(id);
    //    furniture.IsDeleted = true;

    //    return true;
    //}

    //public async Task<IEnumerable<FurnitureForResultDto>> RetrieveAllAsync(PaginationParams @params)
    //{
    //    var furnitures = await _repository.SelectAll()
    //              .Include(f => f.FurnitureFeature)
    //              .AsNoTracking()
    //              .ToPagedList(@params)
    //              .ToListAsync();

    //    return _mapper.Map<IEnumerable<FurnitureForResultDto>>(furnitures);
    //}

    //public async Task<FurnitureForResultDto> RetrieveByIdAsync(long id)
    //{
    //    var byFurniture = await _repository.SelectAll()
    //            .Where(u => u.Id == id)
    //            .Include(f => f.FurnitureFeature)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync() ??
    //                throw new SelenMebelException(404, "Furniture is not found! ");

    //    return _mapper.Map<FurnitureForResultDto>(byFurniture);
    //}

    //public async Task<FurnitureForResultDto> RetrieveByUniqueIdAsync(long uniqueId)
    //{
    //    var byUniqueId = await _repository.SelectAll()
    //            .Where(u => u.UniqueId == uniqueId)
    //            .Include(f => f.FurnitureFeature)
    //            .AsNoTracking()
    //            .FirstOrDefaultAsync();
    //    if (byUniqueId is null)
    //        throw new SelenMebelException(404, "Furniture is not found! ");

    //    return _mapper.Map<FurnitureForResultDto>(byUniqueId);
    //}
}

