﻿using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.FurnitureFeatures;

namespace SelenMebel.Service.Interfaces.FurnitureFeatures;

public interface IFurnitureFeatureService
{
    Task<bool> RemoveAsync(long id);
    Task<FurnitureFeatureForResultDto> RetrieveByIdAsync(long id);
    Task<FurnitureFeatureForResultDto> CreateAsync(FurnitureFeatureForCreationDto dto);
    Task<FurnitureFeatureForResultDto> ModifyAsync(long id, FurnitureFeatureForUpdateDto dto);
    Task<IEnumerable<FurnitureFeatureForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<IEnumerable<FurnitureFeatureForResultDto>> RetrieveAllFeaturesAsync();
    Task<IEnumerable<FurnitureFeatureForResultDto>> RetrieveAllByPropertiesOfFurnitureFeaturesAsync(string searchTerm);
}
