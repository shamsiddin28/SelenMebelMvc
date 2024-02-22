using Microsoft.AspNetCore.Mvc;
using SelenMebel.Service.Helpers;

namespace SelenMebel.Api.Controllers
{
	public class UnitOfController : BaseController
	{

		//[HttpGet("{fileName}")]
		//public async Task<IActionResult> GetImageAsync([FromRoute] string fileName)
		//{
		//	string path = WebHostEnviromentHelper.WebRootPath + "\\Images\\";
		//	var filePath = Path.Combine(path, fileName);
		//	if (System.IO.File.Exists(filePath))
		//	{
		//		byte[] b = System.IO.File.ReadAllBytes(filePath);
		//		return File(b, "image/png");
		//	}
		//	return null;
		//}
	}
}
