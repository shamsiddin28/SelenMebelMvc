using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.Interfaces.Files;

namespace SelenMebelMVC.Areas.Files
{
    public class FilesController : BaseController
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("DownloadImageByPartPath")]
        public async Task<IActionResult> DownloadAsync(string imagePartPath)
        {
            return File(await _fileService.DownloadAsync(imagePartPath), "application/octet-stream", imagePartPath);
        }
    }
}
