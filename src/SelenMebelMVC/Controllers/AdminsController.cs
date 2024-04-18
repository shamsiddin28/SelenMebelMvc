using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Interfaces.Admins;
using SelenMebel.Service.Interfaces.Commons;
using SelenMebel.Service.ViewModels.AdminViewModels;

namespace SelenMebelMVC.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class AdminsController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IIdentityService _identityService;

        public AdminsController(IAdminService adminService, IIdentityService identityService)
        {
            _adminService = adminService;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchValue)
        {
            List<AdminViewModel> admins;
            if (!string.IsNullOrEmpty(searchValue))
            {
                ViewBag.AdminSearch = searchValue;
                admins = await _adminService.GetAllAsync(searchValue);
            }
            else
            {
                admins = await _adminService.GetAllAsync();
            }

            return View(admins);
        }

        public async Task<ViewResult> Profile()
        {
            var admin = await _adminService.GetByTokenAsync();
            if (admin is not null)
                return View(admin);
            return View();
        }

        public async Task<ViewResult> Update(long id)
        {
            try
            {
                var admin = await _adminService.GetByIdAsync(id);

                var adminUpdate = new AdminUpdateDto()
                {
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    Address = admin.Address,
                    BirthDate = admin.BirthDate,
                    PhoneNumber = admin.PhoneNumber,
                    ImagePath = admin.ImagePath,

                };

                return View("Update", adminUpdate);

            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(long id, AdminUpdateDto dto)
        {

            try
            {
                var product = await _adminService.UpdateAsync(id, dto);
                if (product)
                {
                    TempData["SuccessMessage"] = "Admin Updated Successfully !";
                    return RedirectToAction("Profile", "Admins");
                }

                return await Update(id);
            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";

                throw new Exception(ex.Message);
            }
        }

        public async Task<ViewResult> Remove(long id)
        {
            try
            {
                var admin = await _adminService.GetByIdAsync(id);
                if (admin is not null)
                {
                    return View("Remove", admin);
                }
                return View("Remove", id);
            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";
                throw new Exception(ex.Message);
            }
        }

        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> RemoveConfirmed(long id)
        {
            try
            {
                var product = await _adminService.DeleteAsync(id);
                if (product)
                {
                    TempData["SuccessMessage"] = "Admin Removed Successfully !";
                    return RedirectToAction("Index", "Admins");
                }
                return View("Remove", id);
            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";

                throw new Exception(ex.Message);
            }
        }
    }
}
