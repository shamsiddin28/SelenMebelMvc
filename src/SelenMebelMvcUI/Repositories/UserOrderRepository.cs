using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SelenMebelMvcUI.Repositories
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        public UserOrderRepository
            (ApplicationDbContext dbContext, 
             IHttpContextAccessor httpContextAccessor,
             UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders = await _dbContext.Orders
                                         .Include(oD => oD.OrderDetail)
                                         .ThenInclude(f => f.Furniture)
                                         .ThenInclude(c => c.Category)
                                         .Where(u => u.UserId == userId)
                                         .ToListAsync();
            return orders;
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
