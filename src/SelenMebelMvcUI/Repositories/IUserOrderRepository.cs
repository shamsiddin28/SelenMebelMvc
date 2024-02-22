namespace SelenMebelMvcUI.Repositories
{
	public interface IUserOrderRepository
	{
		Task<IEnumerable<Order>> UserOrders();
	}
}