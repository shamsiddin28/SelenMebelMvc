using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.Commons.Security;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Interfaces.Admins;
using SelenMebel.Service.Interfaces.Commons;
using SelenMebel.Service.Interfaces.Files;
using SelenMebel.Service.ViewModels.AdminViewModels;
using System.Net;

namespace SelenMebel.Service.Services.Admins
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork unitOfWork, IIdentityService identityService, IFileService fileService, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._identityService = identityService;
            this._fileService = fileService;
            this._mapper = mapper;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var admin = await _unitOfWork.Admins.SelectByIdAsync(id);
            if (admin is null) throw new NotFoundException("Admin", $"{id} not found");
            admin.IsDeleted = true;
            return await _unitOfWork.Admins.DeleteAsync(id) && await _fileService.DeleteImageAsync(admin.Image);
        }

        public async Task<bool> DeleteImageAsync(long adminId)
        {
            var admin = await _unitOfWork.Admins.SelectByIdAsync(adminId);
            if (admin is null) throw new NotFoundException("Admin", $"{adminId} not found");
            else
            {
                var isDeletedOldImage = await _fileService.DeleteImageAsync(admin.Image!);
                admin.Image = "";
                var result = await _unitOfWork.Admins.UpdateAsync(admin);
                if (result is not null && isDeletedOldImage)
                    return true;
                return false;
            }
        }

        public async Task<List<AdminViewModel>> GetAllAsync(string search)
        {
            var query = _unitOfWork.Admins.SelectAll();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FirstName.ToLower().Contains(search.ToLower())
                || x.LastName.ToLower().Contains(search.ToLower())
                || x.Address.ToLower().Contains(search.ToLower())
                || x.PhoneNumber.ToLower().Contains(search.ToLower()));
            }

            var result = await query.OrderByDescending(x => x.CreatedAt).Select(x => (AdminViewModel)x).ToListAsync();
            return result;
        }

        public Task<List<AdminViewModel>> GetAllAsync()
        {
            var query = _unitOfWork.Admins.SelectAll().OrderByDescending(x => x.CreatedAt).Select(x => (AdminViewModel)x).ToListAsync();
            return query;
        }

        public async Task<AdminViewModel> GetByIdAsync(long id)
        {
            var admin = await _unitOfWork.Admins.SelectByIdAsync(id);
            if (admin is null) throw new NotFoundException("Admin", $"{id} not found");
            var adminView = (AdminViewModel)admin;
            return adminView;
        }

        public async Task<AdminViewModel> GetByTokenAsync()
        {
            var admin = await _unitOfWork.Admins.SelectByIdAsync(long.Parse(_identityService.Id!.Value.ToString()));
            if (admin is null) throw new StatusCodeException(HttpStatusCode.NotFound, "Admin not found!");
            var result = _mapper.Map<AdminViewModel>(admin);
            return result;
        }

        public async Task<AdminViewModel> GetByPhoneNumberAsync(string phoneNumber)
        {
            var admin = await _unitOfWork.Admins.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            if (admin is null) throw new NotFoundException("Admin", $"{phoneNumber} not found");
            var adminView = (AdminViewModel)admin;
            return adminView;
        }

        public async Task<bool> UpdateAsync(long id, AdminUpdateDto adminUpdatedDto)
        {
            var admin = await _unitOfWork.Admins.SelectByIdAsync(id);
            if (admin is null) throw new NotFoundException("Admin", $"{id} not found");
            _unitOfWork.Admins.TrackingDeteched(admin);
            if (adminUpdatedDto != null)
            {
                admin.FirstName = string.IsNullOrEmpty(adminUpdatedDto.FirstName) ? admin.FirstName : adminUpdatedDto.FirstName;
                admin.LastName = string.IsNullOrEmpty(adminUpdatedDto.LastName) ? admin.LastName : adminUpdatedDto.LastName;
                admin.Image = string.IsNullOrEmpty(adminUpdatedDto.ImagePath) ? admin.Image : adminUpdatedDto.ImagePath;
                admin.PhoneNumber = string.IsNullOrEmpty(adminUpdatedDto.PhoneNumber) ? admin.PhoneNumber : adminUpdatedDto.PhoneNumber;
                admin.BirthDate = adminUpdatedDto.BirthDate;
                admin.Address = string.IsNullOrEmpty(adminUpdatedDto.Address) ? admin.Address : adminUpdatedDto.Address;

                var isDeleteOldImage = await _fileService.DeleteImageAsync(admin.Image);

                if (adminUpdatedDto.Image is not null)
                {
                    admin.Image = await _fileService.UploadImageAsync(adminUpdatedDto.Image);
                }
                admin.UpdatedAt = TimeHelper.GetCurrentServerTime();
                var result = await _unitOfWork.Admins.UpdateAsync(admin);
                if (result is not null && isDeleteOldImage)
                    return true;
                return false;
            }
            else throw new ModelErrorException("", "Not found");
        }

        public async Task<bool> UpdateImageAsync(long id, IFormFile formFile)
        {
            var admin = await _unitOfWork.Admins.SelectByIdAsync(id);
            var isDeleteOldImage = await _fileService.DeleteImageAsync(admin.Image);

            var updateImage = await _fileService.UploadImageAsync(formFile);
            var adminUpdatedDto = new AdminUpdateDto()
            {
                ImagePath = updateImage
            };
            return await UpdateAsync(id, adminUpdatedDto) && isDeleteOldImage;
        }

        public async Task<bool> UpdatePasswordAsync(long id, PasswordUpdateDto dto)
        {
            var admin = await _unitOfWork.Admins.SelectByIdAsync(id);
            if (admin is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Admin is not found");
            _unitOfWork.Admins.TrackingDeteched(admin);
            var res = PasswordHasher.Verify(dto.OldPassword, admin.Salt, admin.PasswordHash);
            if (res)
            {
                if (dto.NewPassword == dto.VerifyPassword)
                {
                    var hash = PasswordHasher.Hash(dto.NewPassword);
                    admin.PasswordHash = hash.Hash;
                    admin.Salt = hash.Salt;
                    var result = await _unitOfWork.Admins.UpdateAsync(admin);
                    if (result is not null)
                        return true;
                    return false;
                }
                else throw new StatusCodeException(HttpStatusCode.BadRequest, "new password and verify" + " password must be match!");
            }
            else throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid Password");
        }
    }
}
