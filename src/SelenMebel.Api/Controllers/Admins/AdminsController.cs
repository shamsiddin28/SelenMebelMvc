using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Interfaces.Admins;

namespace SelenMebel.Api.Controllers.Admins
{
    [Authorize(Roles = "superadmin")]
    public class AdminsController : BaseController
    {
        private readonly IAdminService _adminService;

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _adminService.GetAllAsync());

        [HttpGet("GetAdminBySearch")]
        public async Task<IActionResult> GetAllBySearchSAsync(string search)
            => Ok(await _adminService.GetAllAsync(search));

        [Authorize]
        [HttpGet("admin/get-by-token")]
        public async Task<IActionResult> GetByTokenAsync()
            => Ok(await _adminService.GetByTokenAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(await _adminService.GetByIdAsync(id));

        [HttpGet("phoneNumber")]
        public async Task<IActionResult> GetByPhoneNumberAsync(string phoneNumber)
            => Ok(await _adminService.GetByPhoneNumberAsync(phoneNumber));

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] AdminUpdateDto dto)
            => Ok(await _adminService.UpdateAsync(id, dto));

        [HttpPatch("updateImage")]
        public async Task<IActionResult> UpdateImageAsync(long id, IFormFile formFile)
            => Ok(await _adminService.UpdateImageAsync(id, formFile));

        [HttpPost("passwordUpdate")]
        public async Task<IActionResult> UpdatePasswordAsync(long id, PasswordUpdateDto dto)
            => Ok(await _adminService.UpdatePasswordAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(await _adminService.DeleteAsync(id));

        [HttpDelete("deleteImage")]
        public async Task<IActionResult> DeleteImageAsync([FromRoute(Name = "id")] long id)
            => Ok(await _adminService.DeleteImageAsync(id));

    }
}
