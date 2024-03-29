﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SelenMebel.Domain.Entities;

namespace SelenMebelMvcUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dBcontext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(
            ApplicationDbContext dBcontext,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _dBcontext = dBcontext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddItem(long furnitureId, long qty)
        {
            string userId = GetUserId();
            using var transaction = _dBcontext.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _dBcontext.ShoppingCarts.Add(cart);
                }
                await _dBcontext.SaveChangesAsync();
                // cart detail section
                var cartItem = _dBcontext.CartDetails
                                         .FirstOrDefault(s => s.ShoppingCartId == cart.Id
                                         && s.FurnitureId == furnitureId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        FurnitureId = furnitureId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty
                    };
                    _dBcontext.Add(cartItem);
                }
                _dBcontext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(long furnitureId)
        {
            //using var transaction = _dBcontext.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _dBcontext.CartDetails
                                         .FirstOrDefault(s => s.ShoppingCartId == cart.Id
                                         && s.FurnitureId == furnitureId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _dBcontext.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;

                await _dBcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;

        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId is null)
                throw new Exception("Invalid UserId");

            var shoppingCart = await _dBcontext.ShoppingCarts
                                         .Include(c => c.CartDetails)
                                         .ThenInclude(f => f.Furniture)
                                         .ThenInclude(f => f.Category)
                                         .Where(u => u.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _dBcontext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _dBcontext.ShoppingCarts
                              join cartDetail in _dBcontext.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              select new { cartDetail.Id }
                              ).ToListAsync();
            return data.Count;

        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }

        Task<int> ICartRepository.GetCartItemCount(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
