using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.Interfaces.Files;

public interface IFileService
{
    public Task<string> UploadImageAsync(IFormFile image);
    public Task<bool> DeleteImageAsync(string imagePartPath);
}
