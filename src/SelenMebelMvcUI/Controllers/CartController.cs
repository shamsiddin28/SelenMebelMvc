using Microsoft.AspNetCore.Mvc;

namespace SelenMebelMvcUI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(long furnitureId, long qty = 1, long redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(furnitureId, qty);
            if(redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(long furnitureId)
        {
            var cartCount = await _cartRepository.RemoveItem(furnitureId);
            return RedirectToAction($"Remove Item {cartCount}");    
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart(); 
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            long cartItem = await _cartRepository.GetCartItemCount();
            return Ok(cartItem);

        }

    }
}
