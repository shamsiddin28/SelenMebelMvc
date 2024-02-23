using AutoMapper;
using SelenMebel.Service.Helpers;
using SelenMebel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.IRepositories;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Extensions;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.Furnitures;
using SelenMebel.Service.Interfaces.Furnitures;

namespace SelenMebel.Service.Services.Furnitures;

public class FurnitureService : IFurnitureService
{
	private readonly IMapper _mapper;
	private readonly IFurnitureRepository _repository;

	public FurnitureService(IFurnitureRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<FurnitureForResultDto> CreateAsync(FurnitureForCreationDto dto)
	{
		var addFurniture = await _repository.SelectAll()
				.Where(tof => tof.Name.ToLower() == dto.Name.ToLower() && tof.TypeOfFurnitureId == dto.TypeOfFurnitureId)
				.FirstOrDefaultAsync();

		if (addFurniture is not null)
			throw new SelenMebelException(409, "Furniture alredy exists !");

		var image = await MediaHelper.UploadFile(dto.Image, "FurnitureImages");

		var mapped = _mapper.Map<Furniture>(dto);

		mapped.UniqueId = long.Parse(GenerateOtpDigits());
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

		var image = await MediaHelper.UploadFile(dto.Image, "FurnitureImages");

		var mappedFurniture = this._mapper.Map(dto, furniture);

		mappedFurniture.UpdatedAt = DateTime.UtcNow.AddHours(5);
		mappedFurniture.Image = image;
		mappedFurniture.UniqueId = long.Parse(GenerateOtpDigits());

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

		var imageFullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, furniture.Image);

		if (File.Exists(imageFullPath))
			File.Delete(imageFullPath);

		await _repository.DeleteAsync(id);
		furniture.IsDeleted = true;

		return true;
	}

	public async Task<IEnumerable<FurnitureForResultDto>> RetrieveAllAsync()
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
		var byFurniture = await _repository.SelectAll()
				.Where(u => u.Id == id)
				.Include(tOF => tOF.TypeOfFurniture)
				.ThenInclude(c => c.Category)
				.Include(fF => fF.FurnitureFeatures)
				.Include(cD => cD.CartDetail)
				.Include(oD => oD.OrderDetail)
				.AsNoTracking()
				.FirstOrDefaultAsync() ??
					throw new SelenMebelException(404, "Furniture is not found !");

		return _mapper.Map<FurnitureForResultDto>(byFurniture);
	}

	public async Task<FurnitureForResultDto> RetrieveByUniqueIdAsync(long uniqueId)
	{
		var byUniqueId = await _repository.SelectAll()
				.Where(u => u.UniqueId == uniqueId)
				.Include(tOF => tOF.TypeOfFurniture)
				.Include(fF => fF.FurnitureFeatures)
				.Include(cD => cD.CartDetail)
				.Include(oD => oD.OrderDetail)
				.AsNoTracking()
				.FirstOrDefaultAsync();
		if (byUniqueId is null)
			throw new SelenMebelException(404, "Furniture is not found !");

		return _mapper.Map<FurnitureForResultDto>(byUniqueId);
	}
}

