using Microsoft.AspNetCore.Http;
using SelenMebel.Service.Commons.Helpers;
using SelenMebel.Service.Helpers;
using SelenMebel.Service.Interfaces.Files;

namespace SelenMebel.Service.Services.Files
{
    public class FileService : IFileService
    {
        private readonly string ASSETS_FOLDER;
        private readonly string MEDIA_FOLDER;
        private readonly string RESOUCE_IMAGE_FOLDER;
        private readonly string AVATAR_FOLDER;

        public FileService()
        {
            ASSETS_FOLDER = WebHostEnviromentHelper.WebRootPath;
            MEDIA_FOLDER = "media";
            RESOUCE_IMAGE_FOLDER = Path.Combine(MEDIA_FOLDER, "images");
            AVATAR_FOLDER = Path.Combine(MEDIA_FOLDER, "avatars");
        }

        public async Task<bool> DeleteImageAsync(string imagePartPath)
        {
            string path = Path.Combine(ASSETS_FOLDER, imagePartPath);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            string fileName = ImageHelper.UniqueName(image.FileName);
            string partPath = Path.Combine(RESOUCE_IMAGE_FOLDER, fileName);
            string path = Path.Combine(ASSETS_FOLDER, partPath);
            var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);
            stream.Close();
            return partPath;
        }
    }
}
