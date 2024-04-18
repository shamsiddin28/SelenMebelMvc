using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.Interfaces.IRepositories;
using SelenMebel.Domain.Configurations;
using SelenMebel.Domain.Entities.Furnitures;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Extensions;
using SelenMebel.Service.Helpers;
using SelenMebel.Service.Interfaces.Furnitures;

namespace SelenMebel.Service.Services.Furnitures;

public class FurnitureService : IFurnitureService
{
    private readonly IMapper _mapper;
    private readonly string FURNITURE_FOLDER;
    private readonly string RESOUCE_IMAGE_FOLDER;
    private readonly IFurnitureRepository _repository;
    private readonly IHostingEnvironment _hostingEnvironment;

    public FurnitureService(
        IFurnitureRepository repository,
        IMapper mapper,
        IHostingEnvironment hostingEnvironment)
    {
        _mapper = mapper;
        _repository = repository;
        FURNITURE_FOLDER = "FurnitureImages";
        _hostingEnvironment = hostingEnvironment;
        RESOUCE_IMAGE_FOLDER = Path.Combine(FURNITURE_FOLDER, "Images");
    }

    public async Task<FurnitureForResultDto> CreateAsync(FurnitureForCreationDto dto)
    {
        var addFurniture = await _repository.SelectAll()
                .Where(tof => tof.Name.ToLower() == dto.Name.ToLower() && tof.TypeOfFurnitureId == dto.TypeOfFurnitureId)
                .FirstOrDefaultAsync();

        if (addFurniture is not null)
            throw new SelenMebelException(409, "Furniture alredy exists !");

        var image = await MediaHelper.UploadFile(dto.Image, _hostingEnvironment.WebRootPath, RESOUCE_IMAGE_FOLDER);

        var mapped = _mapper.Map<Furniture>(dto);

        mapped.UniqueId = GenerateOtpDigits();
        mapped.CreatedAt = DateTime.UtcNow.AddHours(5);
        mapped.Image = image;

        var result = await _repository.InsertAsync(mapped);

        return _mapper.Map<FurnitureForResultDto>(result);
    }

    public string GenerateOtpDigits()
    {
        string num = "0123456789";
        int len = num.Length;
        string otp = string.Empty;

        int otpDigit = 5;
        string finalDigit;
        int getIndex;
        for (int i = 0; i < otpDigit; i++)
        {
            do
            {
                getIndex = new Random().Next(0, len);
                finalDigit = num.ToCharArray()[getIndex].ToString();
            } while (otp.IndexOf(finalDigit) != -1);
            otp += finalDigit;
        }
        return otp;
    }

    public async Task<FurnitureForResultDto> ModifyAsync(long id, FurnitureForUpdateDto dto)
    {
        var furniture = await _repository.SelectAll()
               .Where(u => u.Id == id)
               .AsNoTracking()
               .FirstOrDefaultAsync();

        if (furniture is null)
            throw new SelenMebelException(404, "Furniture is not found !");

        var image = await MediaHelper.UploadFile(dto.Image, _hostingEnvironment.WebRootPath, RESOUCE_IMAGE_FOLDER);

        var imageFullPath = Path.Combine(_hostingEnvironment.WebRootPath, furniture.Image);

        if (File.Exists(imageFullPath))
            File.Delete(imageFullPath);

        var mappedFurniture = this._mapper.Map(dto, furniture);
        mappedFurniture.UpdatedAt = DateTime.UtcNow.AddHours(5);
        mappedFurniture.Image = image;
        mappedFurniture.UniqueId = GenerateOtpDigits();

        await this._repository.UpdateAsync(mappedFurniture);

        return this._mapper.Map<FurnitureForResultDto>(mappedFurniture);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var furniture = await _repository.SelectAll()
                .Where(u => u.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

        if (furniture is null)
            throw new SelenMebelException(404, "Furniture is not found !");

        var imageFullPath = Path.Combine(_hostingEnvironment.WebRootPath, furniture.Image);

        if (File.Exists(imageFullPath))
            File.Delete(imageFullPath);

        await _repository.DeleteAsync(id);
        furniture.IsDeleted = true;

        return true;
    }

    public async Task<IEnumerable<FurnitureForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var furnitures = await _repository.SelectAll()
                  .Include(tof => tof.TypeOfFurniture)
                  .ThenInclude(c => c.Category)
                  .Include(fF => fF.FurnitureFeatures)
                  .Include(cD => cD.CartDetail)
                  .Include(oD => oD.OrderDetail)
                  .AsNoTracking()
                  .ToPagedListFurniture(@params)
                  .ToListAsync();

        return _mapper.Map<IEnumerable<FurnitureForResultDto>>(furnitures);
    }

    public async Task<IEnumerable<FurnitureForResultDto>> RetrieveAllByPropertiesOfFurnituresAsync(string searchTerm)
    {
        var query = _repository.SelectAll();
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => x.UniqueId.ToLower().Contains(searchTerm.ToLower())
                                || x.Name.ToString().Contains(searchTerm.ToLower())
                                || x.Description.ToString().Contains(searchTerm.ToLower())
                                || x.Price.ToString().Contains(searchTerm.ToLower())
                                || x.Id.ToString().Contains(searchTerm.ToLower()));
        }

        var furniture = await query.OrderByDescending(x => x.CreatedAt)
                                  .Include(tof => tof.TypeOfFurniture)
                                  .ThenInclude(c => c.Category)
                                  .Include(fF => fF.FurnitureFeatures)
                                  .Include(cD => cD.CartDetail)
                                  .Include(oD => oD.OrderDetail)
                                  .AsNoTracking()
                                  .ToListAsync();
        return _mapper.Map<IEnumerable<FurnitureForResultDto>>(furniture);
    }

    public async Task<IEnumerable<FurnitureForResultDto>> RetrieveAllFurnituresAsync()
    {
        var furnitures = await _repository.SelectAll()
                  .Include(tof => tof.TypeOfFurniture)
                  .ThenInclude(c => c.Category)
                  .Include(fF => fF.FurnitureFeatures)
                  .Include(cD => cD.CartDetail)
                  .Include(oD => oD.OrderDetail)
                  .AsNoTracking()
                  .ToListAsync();

        return _mapper.Map<IEnumerable<FurnitureForResultDto>>(furnitures);
    }

    public async Task<FurnitureForResultDto> RetrieveByIdAsync(long id)
    {
        var furniture = await _repository.SelectAll()
                .Where(u => u.Id == id)
                .Include(tOF => tOF.TypeOfFurniture)
                .ThenInclude(c => c.Category)
                .Include(fF => fF.FurnitureFeatures)
                .Include(cD => cD.CartDetail)
                .Include(oD => oD.OrderDetail)
                .AsNoTracking()
                .FirstOrDefaultAsync() ??
                    throw new SelenMebelException(404, "Furniture is not found !");

        return _mapper.Map<FurnitureForResultDto>(furniture);
    }

    public async Task<FurnitureForResultDto> RetrieveByUniqueIdAsync(string uniqueId)
    {
        var byUniqueId = await _repository.SelectAll()
                .Where(u => u.UniqueId.ToLower() == uniqueId.ToLower())
                .Include(tOF => tOF.TypeOfFurniture)
                .Include(fF => fF.FurnitureFeatures)
                .Include(cD => cD.CartDetail)
                .Include(oD => oD.OrderDetail)
                .AsNoTracking()
                .FirstOrDefaultAsync() ??
                    throw new SelenMebelException(404, $"This Furniture {uniqueId} is not found !");

        return _mapper.Map<FurnitureForResultDto>(byUniqueId);
    }
}

