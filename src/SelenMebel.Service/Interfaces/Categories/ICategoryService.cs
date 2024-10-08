﻿using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.Categories;

namespace SelenMebel.Service.Interfaces.Categories;

public interface ICategoryService
{
    Task<bool> RemoveAsync(long id);
    Task<CategoryForResultDto> RetrieveByIdAsync(long id);
    Task<CategoryForResultDto> CreateAsync(CategoryForCreationDto dto);
    Task<CategoryForResultDto> ModifyAsync(long id, CategoryForUpdateDto dto);
    Task<IEnumerable<CategoryForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<IEnumerable<CategoryForResultDto>> RetrieveAllCategoriesAsync();
    Task<IEnumerable<CategoryForResultDto>> RetrieveByPropertiesOfCategoriesAsync(string searchTerm);
}
