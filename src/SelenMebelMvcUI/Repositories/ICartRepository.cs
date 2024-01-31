namespace SelenMebelMvcUI
{
    public interface ICartRepository
    {
        Task<int> AddItem(long furnitureId, long qty);
        Task<int> RemoveItem(long furnitureId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
    }
}