using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.Interfaces.Commons;

public interface IImageService
{
	Task<string> SaveImageAsync(IFormFile file);
	Task<bool> DeleteImageAsync(string imagePath);
}
