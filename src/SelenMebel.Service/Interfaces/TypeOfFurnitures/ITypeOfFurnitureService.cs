using SelenMebel.Domain.Configurations;
using SelenMebel.Service.DTOs.TypeOfFurnitures;

namespace SelenMebel.Service.Interfaces.TypeOfFurnitures;

public interface ITypeOfFurnitureService
{
	Task<bool> RemoveAsync(long id);
	Task<TypeOfFurnitureForResultDto> RetrieveByIdAsync(long id);
	Task<TypeOfFurnitureForResultDto> CreateAsync(TypeOfFurnitureForCreationDto dto);
	Task<TypeOfFurnitureForResultDto> ModifyAsync(long id, TypeOfFurnitureForUpdateDto dto);
	Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllAsync(PaginationParams @params);
	Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllTypeOfFurnituresAsync();
	Task<IEnumerable<TypeOfFurnitureForResultDto>> RetrieveAllByPropertiesOfTypeOfFurnituresAsync(string searchTerm);
}
