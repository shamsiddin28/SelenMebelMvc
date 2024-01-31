using SelenMebel.Domain.Entities;

namespace SelenMebelMvcUI
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Furniture>> GetFurnitures(string sTerm = "", long categoryId = 0);
        Task<IEnumerable<Category>> Categories();
    }
}