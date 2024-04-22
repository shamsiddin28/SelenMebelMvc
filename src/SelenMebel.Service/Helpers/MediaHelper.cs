using Microsoft.AspNetCore.Http;
using SelenMebel.Service.Commons.Helpers;

namespace SelenMebel.Service.Helpers;

public static class MediaHelper
{
	public static async Task<string> UploadFile(IFormFile file, string webRootPath, string imagePartPath)
	{
		if (file != null && file.Length > 0)
		{
			string fileName = ImageHelper.UniqueName(file.FileName);

			string partPath = Path.Combine(imagePartPath, fileName);
			string path = Path.Combine(webRootPath, partPath);
			var stream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(stream);
			stream.Close();
			return partPath;
		}
		return null;
	}
}
