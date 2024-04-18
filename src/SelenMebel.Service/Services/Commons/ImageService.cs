using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.Interfaces.Commons;

namespace SelenMebel.Service.Services.Commons;

public class ImageService : IImageService
{
    private readonly string images = "media/images";
    private readonly IHostingEnvironment _hostingEnvironment;
    public ImageService(IHostingEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public Task<bool> DeleteImageAsync(string imagePath)
    {
        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, imagePath);
        if (!File.Exists(filePath)) return Task.FromResult(false);

        try
        {
            File.Delete(filePath);
            return Task.FromResult(true);
        }
        catch (Exception)
        {
            return Task.FromResult(false);
        }
    }

    public async Task<string> SaveImageAsync(IFormFile file)
    {
        string ImageName = ImageHelper.UniqueName(file.FileName);
        string ImagePath = Path.Combine(_hostingEnvironment.WebRootPath, images, ImageName);
        var stream = new FileStream(ImagePath, FileMode.Create);
        try
        {
            await file.CopyToAsync(stream);
            return Path.Combine(images, ImageName);
        }
        catch
        {

            return "";
        }
        finally
        {
            stream.Close();
        }
    }
}
