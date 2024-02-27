using AutoMapper;
using SelenMebel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.IRepositories;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Extensions;
using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.FurnitureFeatures;
using SelenMebel.Service.Interfaces.FurnitureFeatures;

namespace SelenMebel.Service.Services.FurnitureFeatures;

public class FurnitureFeatureService : IFurnitureFeatureService
{

	private readonly IMapper _mapper;
	private readonly IFurnitureFeatureRepository _furnitureFeatureRepository;
	private readonly IFurnitureRepository _furnitureRepository;

	public FurnitureFeatureService(
		IMapper mapper,
		IFurnitureFeatureRepository FurnitureFeatureRepository,
		IFurnitureRepository furnitureRepository
		)
	{
		_mapper = mapper;
		_furnitureFeatureRepository = FurnitureFeatureRepository;
		_furnitureRepository = furnitureRepository;
	}

	public async Task<FurnitureFeatureForResultDto> CreateAsync(FurnitureFeatureForCreationDto dto)
	{
		var furniture = await _furnitureRepository.SelectAll()
				.Where(tof => tof.Id == dto.FurnitureId)
				.FirstOrDefaultAsync();

		if (furniture is null)
			throw new SelenMebelException(404, "Furniture is not found !");

		var addFurnitureFeature = await _furnitureFeatureRepository.SelectAll()
				.Where(fF => fF.Name.ToLower() == dto.Name.ToLower() && fF.Value.ToLower() == dto.Value.ToLower())
				.FirstOrDefaultAsync();

		if (addFurnitureFeature is not null)
			throw new SelenMebelException(409, "FurnitureFeature already exists !");

		var mapped = _mapper.Map<FurnitureFeature>(dto);
		mapped.CreatedAt = DateTime.UtcNow.AddHours(5);

		var result = await _furnitureFeatureRepository.InsertAsync(mapped);

		return _mapper.Map<FurnitureFeatureForResultDto>(result);
	}

	public async Task<FurnitureFeatureForResultDto> ModifyAsync(long id, FurnitureFeatureForUpdateDto dto)
	{
		var furniture = await _furnitureRepository.SelectAll()
				.Where(tof => tof.Id == dto.FurnitureId)
				.FirstOrDefaultAsync();

		if (furniture is null)
			throw new SelenMebelException(404, "Furniture is not found !");

		var addFurnitureFeature = await _furnitureFeatureRepository.SelectAll()
				.Where(fF => fF.Id == id)
				.FirstOrDefaultAsync();

		if (addFurnitureFeature is null)
			throw new SelenMebelException(404, "FurnitureFeature is not found !");


		var mappedFurnitureFeature = this._mapper.Map(dto, addFurnitureFeature);
		mappedFurnitureFeature.UpdatedAt = DateTime.UtcNow.AddHours(5);

		await _furnitureFeatureRepository.UpdateAsync(mappedFurnitureFeature);

		return _mapper.Map<FurnitureFeatureForResultDto>(mappedFurnitureFeature);
	}

	public async Task<bool> RemoveAsync(long id)
	{
		var furnitureFeature = await _furnitureFeatureRepository.SelectAll()
				.Where(u => u.Id == id)
				.AsNoTracking()
				.FirstOrDefaultAsync();

		if (furnitureFeature is null)
			throw new SelenMebelException(404, "FurnitureFeature is not found !");

		await _furnitureFeatureRepository.DeleteAsync(id);

		return true;
	}

	public async Task<IEnumerable<FurnitureFeatureForResultDto>> RetrieveAllAsync(PaginationParams @params)
	{
		var furnitureFeatures = await _furnitureFeatureRepository.SelectAll()
				  .Include(f => f.Furniture)
				  .ThenInclude(typ => typ.TypeOfFurniture)
				  .ThenInclude(c => c.Category)
				  .AsNoTracking()
				  .ToPagedListFurnitureFeature(@params)
				  .ToListAsync();

		return _mapper.Map<IEnumerable<FurnitureFeatureForResultDto>>(furnitureFeatures);
	}

	public async Task<FurnitureFeatureForResultDto> RetrieveByIdAsync(long id)
	{
		var furnitureFeature = await _furnitureFeatureRepository.SelectAll()
				.Where(u => u.Id == id)
				.Include(f => f.Furniture)
                .ThenInclude(typ => typ.TypeOfFurniture)
                .ThenInclude(c => c.Category)
                .AsNoTracking()
				.FirstOrDefaultAsync() ??
					throw new SelenMebelException(404, "FurnitureFeature is not found !");

		return _mapper.Map<FurnitureFeatureForResultDto>(furnitureFeature);
	}

}
