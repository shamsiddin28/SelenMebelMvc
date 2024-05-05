using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.Interfaces.Files;

namespace SelenMebel.Service.Services.Files
{
    public class FileService : IFileService
    {
        private readonly string MEDIA_FOLDER;
        private readonly string RESOUCE_IMAGE_FOLDER;
        private readonly string AVATAR_FOLDER;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileService(IHostingEnvironment hostingEnvironment)
        {
            MEDIA_FOLDER = "media";
            RESOUCE_IMAGE_FOLDER = Path.Combine(MEDIA_FOLDER, "images");
            AVATAR_FOLDER = Path.Combine(MEDIA_FOLDER, "avatars");
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<bool> DeleteImageAsync(string imagePartPath)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, imagePartPath);

            if (File.Exists(path))
            {
                try
                {
                    // Asynchronously delete the file by offloading the blocking operation to a background thread
                    await Task.Run(() => File.Delete(path));
                    return true;
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<byte[]> DownloadAsync(string imagePartPath)
        {
            var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, imagePartPath);
            if (File.Exists(imagePath))
                return await File.ReadAllBytesAsync(imagePath);

            throw new FileNotFoundException();
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            string fileName = ImageHelper.UniqueName(image.FileName);
            string partPath = Path.Combine(RESOUCE_IMAGE_FOLDER, fileName);
            string path = Path.Combine(_hostingEnvironment.WebRootPath, partPath);
            var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);
            stream.Close();
            return partPath;
        }
    }
}
