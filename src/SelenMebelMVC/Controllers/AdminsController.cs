using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Interfaces.Admins;
using SelenMebel.Service.Interfaces.Commons;
using SelenMebel.Service.ViewModels.AdminViewModels;

namespace SelenMebelMVC.Controllers
{
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
        [Authorize(Roles = "superadmin")]
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

        [Authorize(Roles = "superadmin")]
        public async Task<ViewResult> Profile()
        {
            var admin = await _adminService.GetByTokenAsync();
            if (admin is not null)
                return View(admin);
            return View();
        }

        [Authorize(Roles = "superadmin")]
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
                ViewBag.AdminId = admin.Id;
                return View("Update", adminUpdate);

            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Update(long id, AdminUpdateDto dto)
        {

            try
            {
                var updatedAdmin = await _adminService.UpdateAsync(id, dto);
                if (updatedAdmin)
                {
                    TempData["SuccessMessage"] = "Admin Updated Successfully !";
                    return RedirectToAction(nameof(Index), "Admins");
                }

                return await Update(id);
            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";

                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "superadmin")]
        public async Task<ViewResult> Remove(long id)
        {
            try
            {
                var admin = await _adminService.GetByIdAsync(id);
                if (admin is not null)
                {
                    return View("Remove", admin);
                }
                return View("Remove", admin);
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
                var isDeletedAdmin = await _adminService.DeleteAsync(id);
                if (isDeletedAdmin)
                {
                    TempData["SuccessMessage"] = "Admin Removed Successfully !";
                    return RedirectToAction("Index", "Admins");
                }
                else
                {
                    return View("Remove", id);
                }
            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";

                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "superadmin")]
        public async Task<ViewResult> UpdatePassword(long id)
        {
            try
            {
                var admin = await _adminService.GetByIdAsync(id);

                var passwordUpdateDto = new PasswordUpdateDto();

                return View(nameof(UpdatePassword), passwordUpdateDto);

            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> UpdatePassword(long id, PasswordUpdateDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _adminService.UpdatePasswordAsync(id, dto);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Admin Password Updated Successfully !";
                        return await Update(id);
                    }
                    else
                    {
                        TempData["InfoMessage"] = "Invalid password!";
                        return View(nameof(UpdatePassword), id);
                    }
                }
                else
                {
                    ModelState.Clear();
                    TempData["InfoMessage"] = "Please provide all the required fields!";
                    return View(nameof(UpdatePassword), id);
                }
            }
            catch (Exception ex)
            {
                TempData["InfoMessage"] = $"{ex.Message}";
                throw new Exception(ex.Message);
            }

        }
    }
}
