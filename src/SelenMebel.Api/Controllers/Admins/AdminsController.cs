using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Interfaces.Admins;
using SelenMebel.Service.Interfaces.Commons;
using SelenMebel.Service.ViewModels.AdminViewModels;

namespace SelenMebel.Api.Controllers.Admins
{
    [Route("admins")]
    [Authorize(Roles = "superadmin")]
    public class AdminsController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly IIdentityService _identityService;

        public AdminsController(IAdminService adminService, IIdentityService identityService)
        {
            this._adminService = adminService;
            this._identityService = identityService;
        }

        [HttpGet("GetAllAdmins")]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await this._adminService.GetAllAsync());

        #region GetAll
        [HttpGet("GetAdminBySearch")]
        public async Task<IActionResult> GetAllBySearchSAsync(string search)
        {
            List<AdminViewModel> admins;
            if (!string.IsNullOrEmpty(search))
            {
                admins = await _adminService.GetAllAsync(search);
            }
            else
            {
                admins = await _adminService.GetAllAsync();
            }

            return Ok(admins);
        }
        #endregion

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute(Name = "id")] long id)
            => Ok(await _adminService.GetByIdAsync(id));

        #region GetPhoneNumber
        [HttpGet("phoneNumber")]
        public async Task<IActionResult> GetByPhoneNumberAsync(string phoneNumber)
        {
            var admin = await _adminService.GetByPhoneNumberAsync(phoneNumber);
            var adminView = new AdminViewModel()
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                ImagePath = admin.ImagePath,
                PhoneNumber = admin.PhoneNumber,
                BirthDate = admin.BirthDate,
                Address = admin.Address,
                CreatedAt = admin.CreatedAt
            };

            return Ok(adminView);
        }
        #endregion

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute(Name = "id")] long id, [FromForm] AdminUpdateDto dto)
            => Ok(await this._adminService.UpdateAsync(id, dto));

        [HttpPost("updateImage")]
        public async Task<IActionResult> UpdateImageAsync(long id, [FromForm] IFormFile formFile)
            => Ok(await this._adminService.UpdateImageAsync(id, formFile));

        [HttpPost("passwordUpdate")]
        public async Task<IActionResult> UpdatePasswordAsync(long id, PasswordUpdateDto dto)
            => Ok(await this._adminService.UpdatePasswordAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] long id)
            => Ok(await this._adminService.DeleteAsync(id));

        [HttpDelete("deleteImage")]
        public async Task<IActionResult> DeleteImageAsync([FromRoute(Name = "id")] long id)
            => Ok(await this._adminService.DeleteImageAsync(id));

    }
}
