using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.DTOs.Accounts;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Interfaces.Accounts;

namespace SelenMebelMVC.Controllers
{
	public class AccountsController : Controller
	{
		private readonly IAccountService _accountService;

		public AccountsController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet("register")]
		[Authorize(Roles = "admin, superadmin")]
		public ViewResult Register() => View("Register");

		[HttpPost("register")]
		[Authorize(Roles = "admin, superadmin")]
		public async Task<IActionResult> AdminRegisterAsync(AdminRegisterDto adminRegisterDto)
		{
			if (ModelState.IsValid)
			{
				bool result = await _accountService.AdminRegisterAsync(adminRegisterDto);
				if (result)
				{
					return RedirectToAction("logout", "accounts", new { area = "" });
				}
				else
				{
					TempData["InfoMessage"] = "Please provide all the required fields";
					return Register();
				}
			}
			else
			{
				TempData["InfoMessage"] = "Please provide all the required fields";
				return Register();
			}
		}

		[HttpGet("login")]
		public ViewResult Login() => View("Login");

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> LoginAsync(AccountLoginDto accountLoginDto)
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
					TempData["SuccessMessage"] = $"You have successfully entered the admin panel";

					return RedirectToAction("Index", "Furniture");
				}
				catch (ModelErrorException modelError)
				{
					TempData["InfoMessage"] = $"You have entered an incorrect password or login!";
					TempData["ErrorMessage"] = "An error occurred: " + modelError.Message;
					ModelState.AddModelError(modelError.Property, modelError.Message);
					return Login();
				}
				catch (Exception ex)
				{
					TempData["ErrorMessage"] = "An error occurred: " + ex.Message;

					return Login();
				}
			}
			else return View(nameof(Login));
		}

		[HttpGet("logout")]
		public IActionResult LogOut()
		{
			HttpContext.Response.Cookies.Append("X-Access-Token", "", new CookieOptions()
			{
				Expires = TimeHelper.GetCurrentServerTime().AddDays(-1)
			});
			return View(nameof(Login));
		}

	}
}

