using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.DTOs.Accounts;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.DTOs.Users;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Interfaces.Users;

namespace SelenMebel.Api.Controllers.Users
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> RegisterAsync([FromForm] UserRegisterDto dto)
        => Ok(await this._userService.UserRegisterAsync(dto));

        [HttpPost("user/login")]
        public async Task<IActionResult> LoginAsync([FromForm] AccountLoginDto accountLoginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = await _userService.LoginAsync(accountLoginDto);
                    HttpContext.Response.Cookies.Append("X-Access-Token", token, new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    });
                    return Ok(token);
                }
                catch (ModelErrorException modelError)
                {
                    ModelState.AddModelError(modelError.Property, modelError.Message);
                    return RedirectToAction("Login", "Users");
                }
                catch
                {
                    return RedirectToAction("Login", "Users");
                }
            }
            else return RedirectToAction("Login", "Users");
        }

        [HttpGet("user/log-out")]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions()
            {
                Expires = TimeHelper.GetCurrentServerTime().AddDays(-1)
            });
            return Ok("LogOut !");
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(await _userService.GetByIdAsync(id));

        [Authorize]
        [HttpGet("user/get-by-token")]
        public async Task<IActionResult> GetByTokenAsync()
            => Ok(await _userService.GetByTokenAsync());

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] UserUpdateDto dto)
            => Ok(await this._userService.UpdateAsync(id, dto));

        [Authorize]
        [HttpPatch("user/update-image/{id}")]
        public async Task<IActionResult> UpdateImageAsync(long id, IFormFile formFile)
            => Ok(await this._userService.UpdateImageAsync(id, formFile));

        [Authorize]
        [HttpPost("user/password-update")]
        public async Task<IActionResult> UpdatePasswordAsync(long id, PasswordUpdateDto dto)
            => Ok(await this._userService.UpdatePasswordAsync(id, dto));

        [Authorize]
        [HttpDelete("delete/user/{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(await this._userService.DeleteAsync(id));

        [Authorize]
        [HttpDelete("user/delete-image/{id}")]
        public async Task<IActionResult> DeleteImageAsync([FromRoute(Name = "id")] long id)
            => Ok(await this._userService.DeleteImageAsync(id));

    }
}
