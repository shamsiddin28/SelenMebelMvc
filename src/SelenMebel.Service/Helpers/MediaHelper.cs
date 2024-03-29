﻿using Microsoft.AspNetCore.Http;

namespace SelenMebel.Service.Helpers;

public class MediaHelper
{
    public static async Task<string> UploadFile(IFormFile file, string imagePath)
    {
        string uniqueFileName = "";
        if (file != null && file.Length > 0)
        {
            string uploadsFolder = Path.Combine(WebHostEnviromentHelper.WebRootPath, imagePath, "Images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string imageFilePath = Path.Combine(uploadsFolder, uniqueFileName);
            uniqueFileName = imageFilePath;
            using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        return uniqueFileName;
    }
}
