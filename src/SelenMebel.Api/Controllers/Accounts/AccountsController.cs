using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.DTOs.Accounts;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Interfaces.Accounts;

namespace SelenMebel.Api.Controllers.Accounts
{
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("admin/register")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostAsync([FromForm] AdminRegisterDto dto)
            => Ok(await this._accountService.AdminRegisterAsync(dto));

        [HttpPost("admin/login")]
        public async Task<IActionResult> LoginAsync([FromForm] AccountLoginDto accountLoginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = await _accountService.LoginAsync(accountLoginDto);
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
                    return RedirectToAction("Login", "Accounts");
                }
                catch
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
            else return RedirectToAction("Login", "Accounts");
        }
        
        [HttpGet("admin/log-out")]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions()
            {
                Expires = TimeHelper.GetCurrentServerTime().AddDays(-1)
            });
            return Ok("LogOut !");
        }
    }
}
