using AutoMapper;
using Microsoft.AspNetCore.Http;
using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Domain.Entities;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.Commons.Security;
using SelenMebel.Service.DTOs.Accounts;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.DTOs.Users;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Interfaces.Commons;
using SelenMebel.Service.Interfaces.Files;
using SelenMebel.Service.Interfaces.Users;
using SelenMebel.Service.ViewModels.UserViewModels;
using System.Net;

namespace SelenMebel.Service.Services.Users
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _repository;
		private readonly IAuthService _authService;
		private readonly IFileService _fileService;
		private readonly IImageService _imageService;
		private readonly IMapper _mapper;
		private readonly IIdentityService _identityService;

		public UserService
			(IUnitOfWork unitOfWork,
			 IAuthService authService,
			 IImageService imageService,
			 IMapper mapper,
			 IIdentityService identityService,
			 IFileService fileService)
		{
			this._repository = unitOfWork;
			this._authService = authService;
			this._imageService = imageService;
			this._mapper = mapper;
			this._identityService = identityService;
			this._fileService = fileService;
		}

		public async Task<bool> UpdateImageAsync(long id, IFormFile path)
		{
			var user = await _repository.Users.SelectByIdAsync(id);
			if (user == null)
				throw new StatusCodeException(HttpStatusCode.NotFound, "User is not found");
			_repository.Users.TrackingDeteched(user);

			var checkImage = false;
			if (user.Image != null)
			{
				checkImage = await _imageService.DeleteImageAsync(user.Image);
			}

			var updatedImage = await _imageService.SaveImageAsync(path);
			var userUpdateDto = new UserUpdateDto()
			{
				ImagePath = updatedImage
			};
			return await UpdateAsync(id, userUpdateDto) && checkImage;
		}

		public async Task<bool> DeleteImageAsync(long id)
		{
			var user = await _repository.Users.SelectByIdAsync(id);
			var isDeletedImage = await _imageService.DeleteImageAsync(user.Image);
			user.Image = "";
			var result = await _repository.Users.UpdateAsync(user);
			return result is not null && isDeletedImage;
		}

		public async Task<string> LoginAsync(AccountLoginDto accountLoginDto)
		{
			var user = await _repository.Users.FirstOrDefault(x => x.PhoneNumber == accountLoginDto.PhoneNumber);
			if (user is null)
				throw new NotFoundException(nameof(accountLoginDto.PhoneNumber), "No user with this phone number is found!");
			else
			{
				var hasherResult = PasswordHasher.Verify(accountLoginDto.Password, user.Salt, user.PasswordHash);
				if (hasherResult)
				{
					string token = _authService.GenerateToken(user, "user");
					return token;
				}
				else throw new NotFoundException(nameof(accountLoginDto.Password), "Incorrect password!");
			}

		}

		public async Task<UserViewModel> GetByIdAsync(long id)
		{
			var user = await _repository.Users.SelectByIdAsync(id);
			if (user is null) throw new StatusCodeException(HttpStatusCode.NotFound, "User is not found!");
			var res = (UserViewModel)user;
			return res;
		}

		public async Task<UserViewModel> GetByTokenAsync()
		{
			var user = await _repository.Users.SelectByIdAsync(long.Parse(_identityService.Id!.Value.ToString()));
			if (user is null) throw new StatusCodeException(HttpStatusCode.NotFound, "User not found!");
			var result = (UserViewModel)user;
			return result;
		}

		public async Task<bool> UpdatePasswordAsync(long id, PasswordUpdateDto dto)
		{
			var user = await _repository.Users.SelectByIdAsync(id);
			if (user is null)
				throw new StatusCodeException(HttpStatusCode.NotFound, "User is not found");
			_repository.Users.TrackingDeteched(user);
			var res = PasswordHasher.Verify(dto.OldPassword, user.Salt, user.PasswordHash);
			if (res)
			{
				if (dto.NewPassword == dto.VerifyPassword)
				{
					var hash = PasswordHasher.Hash(dto.NewPassword);
					user.PasswordHash = hash.Hash;
					user.Salt = hash.Salt;
					var result = await _repository.Users.UpdateAsync(user);
					if (result is not null)
						return true;
					return false;
				}
				else throw new StatusCodeException(HttpStatusCode.BadRequest, "new password and verify" + " password must be match!");
			}
			else throw new StatusCodeException(HttpStatusCode.BadRequest, "Invalid Password");
		}

		public async Task<bool> DeleteAsync(long id)
		{
			var user = await _repository.Users.SelectByIdAsync(id);
			if (user is null) throw new NotFoundException("User", $"{id} not found");
			var isDeletedOldImage = await DeleteImageAsync(id);
			var isDeletedUser = await _repository.Users.DeleteAsync(id);
			return isDeletedUser && isDeletedOldImage;
		}

		public async Task<bool> UpdateAsync(long id, UserUpdateDto userUpdateDto)
		{
			var user = await _repository.Users.SelectByIdAsync(id);
			if (user is null) throw new NotFoundException("User", $"{id} not found");
			_repository.Users.TrackingDeteched(user);
			if (userUpdateDto != null)
			{
				user.FirstName = string.IsNullOrEmpty(userUpdateDto.FirstName) ? user.FirstName : userUpdateDto.FirstName;
				user.LastName = string.IsNullOrEmpty(userUpdateDto.LastName) ? user.LastName : userUpdateDto.LastName;
				user.Image = string.IsNullOrEmpty(userUpdateDto.ImagePath) ? user.Image : userUpdateDto.ImagePath;
				user.PhoneNumber = string.IsNullOrEmpty(userUpdateDto.PhoneNumber) ? user.PhoneNumber : userUpdateDto.PhoneNumber;
				user.Email = string.IsNullOrEmpty(userUpdateDto.Email) ? user.Email : userUpdateDto.Email;
				user.BirthDate = user.BirthDate;

				if (userUpdateDto.Image is not null)
				{
					user.Image = await _fileService.UploadImageAsync(userUpdateDto.Image);
				}
				user.UpdatedAt = TimeHelper.GetCurrentServerTime();
				var result = await _repository.Users.UpdateAsync(user);
				return result is not null;
			}
			else throw new ModelErrorException("", "Not found");
		}

		// Email service yozish kerak verification qismini 
		public async Task<bool> UserRegisterAsync(UserRegisterDto userRegisterDto)
		{
			var phoneNumberCheck = await _repository.Users.FirstOrDefault(x => x.PhoneNumber == userRegisterDto.PhoneNumber);
			if (phoneNumberCheck is not null)
				throw new AlreadyExistingException(nameof(userRegisterDto.PhoneNumber), "This phone number is already registered.");

			var hashresult = PasswordHasher.Hash(userRegisterDto.Password);
			var user = _mapper.Map<User>(userRegisterDto);
			user.PasswordHash = hashresult.Hash;
			user.Salt = hashresult.Salt;
			user.CreatedAt = TimeHelper.GetCurrentServerTime();

			var result = await _repository.Users.InsertAsync(user);

			if (result is not null)
				return true;
			return false;

		}
	}
}
