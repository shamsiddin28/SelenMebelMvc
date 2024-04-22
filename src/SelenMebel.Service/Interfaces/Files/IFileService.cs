using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.Interfaces.Files;

public interface IFileService
{
	Task<string> UploadImageAsync(IFormFile image);
	Task<bool> DeleteImageAsync(string imagePartPath);
	Task<byte[]> DownloadAsync(string imagePartPath);
}
