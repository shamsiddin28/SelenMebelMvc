using MathNet.Numerics.Statistics.Mcmc;
using SelenMebel.Domain.Entities;

namespace SelenMebel.Data.IRepositories
{
	public interface IFurnitureRepository
	{
		IQueryable<Furniture> SelectAll();
		Task<bool> DeleteAsync(long id);
		Task<Furniture> SelectByIdAsync(long id);
		Task<Furniture> InsertAsync(Furniture furniture);
		Task<Furniture> UpdateAsync(Furniture furniture);
	}
}
