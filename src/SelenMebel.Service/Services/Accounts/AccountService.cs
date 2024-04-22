using AutoMapper;
using SelenMebel.Data.Interfaces.Commons;
using SelenMebel.Domain.Entities.Admins;
using SelenMebel.Domain.Enums;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.Commons.Security;
using SelenMebel.Service.DTOs.Accounts;
using SelenMebel.Service.DTOs.Admins;
using SelenMebel.Service.Exceptions;
using SelenMebel.Service.Interfaces.Accounts;
using SelenMebel.Service.Interfaces.Commons;

namespace SelenMebel.Service.Services.Accounts;

public class AccountService : IAccountService
{
	private readonly IUnitOfWork _repository;
	private readonly IAuthService _authService;
	private readonly IMapper _mapper;
	private readonly IImageService _imageService;

	public AccountService(IUnitOfWork unitOfWork, IAuthService authService, IMapper mapper, IImageService imageService)
	{
		this._repository = unitOfWork;
		this._authService = authService;
		this._mapper = mapper;
		this._imageService = imageService;
	}

	public async Task<bool> AdminRegisterAsync(AdminRegisterDto adminRegisterDto)
	{
		var phoneNumberCheck = await _repository.Admins.FirstOrDefault(x => x.PhoneNumber == adminRegisterDto.PhoneNumber);
		if (phoneNumberCheck is not null)
			throw new AlreadyExistingException(nameof(adminRegisterDto.PhoneNumber), "This phone number is already registered.");

		var hashResult = PasswordHasher.Hash(adminRegisterDto.Password);
		var admin = _mapper.Map<Admin>(adminRegisterDto);
		admin.AdminRole = Role.Admin;
		admin.PasswordHash = hashResult.Hash;
		admin.Salt = hashResult.Salt;
		admin.CreatedAt = TimeHelper.GetCurrentServerTime();

		var result = await _repository.Admins.InsertAsync(admin);
		if (result is not null)
			return true;
		return false;
	}

	public async Task<string> LoginAsync(AccountLoginDto accountLoginDto)
	{
		var admin = await _repository.Admins.FirstOrDefault(x => x.PhoneNumber == accountLoginDto.PhoneNumber);
		if (admin is null)
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
		else if (admin.AdminRole == Role.SuperAdmin)
		{
			var hasherResult = PasswordHasher.Verify(accountLoginDto.Password, admin.Salt, admin.PasswordHash);
			if (hasherResult)
			{
				string token = "";
				if (admin.PhoneNumber != null)
				{
					token = _authService.GenerateToken(admin, "superadmin");
					return token;
				}
				token = _authService.GenerateToken(admin, "superadmin");
				return token;
			}
			else throw new NotFoundException(nameof(accountLoginDto.Password), "Incorrect password!");
		}
		else
		{
			var hasherResult = PasswordHasher.Verify(accountLoginDto.Password, admin.Salt, admin.PasswordHash);
			if (hasherResult)
			{
				string token = "";
				if (admin.PhoneNumber != null)
				{
					token = _authService.GenerateToken(admin, "admin");
					return token;
				}
				token = _authService.GenerateToken(admin, "admin");
				return token;
			}
			else throw new NotFoundException(nameof(accountLoginDto.Password), "Incorrect password!");
		}
	}

}
