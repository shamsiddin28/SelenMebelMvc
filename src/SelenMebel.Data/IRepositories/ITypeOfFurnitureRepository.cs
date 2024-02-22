using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.IRepositories;

public interface ITypeOfFurnitureRepository
{
	IQueryable<TypeOfFurniture> SelectAll();
	Task<bool> DeleteAsync(long id);
	Task<TypeOfFurniture> SelectByIdAsync(long id);
	Task<TypeOfFurniture> InsertAsync(TypeOfFurniture typeOfFurniture);
	Task<TypeOfFurniture> UpdateAsync(TypeOfFurniture typeOfFurniture);
}
