using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.Furnitures;

namespace SelenMebel.Service.Interfaces.Furnitures;

public interface IFurnitureService
{
    string GenerateOtpDigits();
    Task<bool> RemoveAsync(long id);
    Task<FurnitureForResultDto> RetrieveByIdAsync(long id);
    Task<FurnitureForResultDto> RetrieveByUniqueIdAsync(string uniqueId);
    Task<FurnitureForResultDto> CreateAsync(FurnitureForCreationDto dto);
    Task<FurnitureForResultDto> ModifyAsync(long id, FurnitureForUpdateDto dto);
    Task<IEnumerable<FurnitureForResultDto>> RetrieveAllAsync(PaginationParams @params);
    Task<IEnumerable<FurnitureForResultDto>> RetrieveAllFurnituresAsync();
    Task<IEnumerable<FurnitureForResultDto>> RetrieveAllByPropertiesOfFurnituresAsync(string searchTerm);
}

