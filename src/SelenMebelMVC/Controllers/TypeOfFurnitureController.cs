using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SelenMebel.Domain.Enums;
using SelenMebel.Service.DTOs.Categories;
using SelenMebel.Service.DTOs.TypeOfFurnitures;
using SelenMebelMVC.Models;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace SelenMebelMVC.Controllers
{
    public class TypeOfFurnitureController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string baseURL = "https://localhost:7200/api/";


        public TypeOfFurnitureController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string errorMessage = "";
        public string successMessage = "";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IList<TypeOfFurnitureForResultDto> typeOfFurnitures = new List<TypeOfFurnitureForResultDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("TypeOfFurnitures");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    typeOfFurnitures = JsonConvert.DeserializeObject<List<TypeOfFurnitureForResultDto>>(results).OrderByDescending(f => f.Id).ToList();

                    return View(typeOfFurnitures);
                }
                else
                {
                    return View(null);
                }

            }
        }

        [NonAction]
        private void LoadTypeOfFurniture(IList<TypeOfFurnitureForResultDto> resultDtos)
        {
            resultDtos = new List<TypeOfFurnitureForResultDto>();
            ViewBag.TypeOfSelen = new SelectList(resultDtos.Select(f => f.TypeOfSelen));
            var types = new List<SelectListItem>();
            foreach (TypeOfSelen type in Enum.GetValues(typeof(TypeOfSelen)))
            {
                types.Add(new SelectListItem
                {
                    Text = type.ToString(),
                    Value = ((int)type).ToString()
                });
            }

            ViewBag.Types = types;

        }


        [HttpGet]
        public async Task<ViewResult> Create()
        {
            var categories = new List<CategoryForResultDto>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("Categories");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    categories = JsonConvert.DeserializeObject<List<CategoryForResultDto>>(results).OrderByDescending(f => f.Id).ToList();

                    ViewBag.Categories = categories;
                    TypeOfFurnitureForCreationDto model = new TypeOfFurnitureForCreationDto();
                    return View("Create", model);
                }
                else
                {
                    return View(null);
                }

            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(TypeOfFurnitureForCreationDto model)
        {
            try
            {
                if (model.CategoryId > 0)
                {
                    if (model.Image == null)
                    {
                        ModelState.AddModelError("TypeOfFurnitureForCreationDto.Image", "The image file is required");
                    }

                    if (ModelState.IsValid)
                    {
                        var apiClient = _httpClientFactory.CreateClient("client");
                        var apiUrl = apiClient.BaseAddress + "api/TypeOfFurnitures";

                        using (var multipartContent = new MultipartFormDataContent())
                        {

                            multipartContent.Add(new StringContent(model.TypeOfSelen.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "typeOfSelen");
                            multipartContent.Add(new StringContent(model.CategoryId.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "categoryId");

                            var imageContent = new StreamContent(model.Image.OpenReadStream());
                            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                            multipartContent.Add(imageContent, "Image", model.Image.FileName);

                            var response = await apiClient.PostAsync(apiUrl, multipartContent);
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                ViewBag.Message = responseContent;
                                TypeOfFurnitureForResultDto data = JsonConvert.DeserializeObject<TypeOfFurnitureForResultDto>(responseContent);

                                model.TypeOfSelen = TypeOfSelen.None;
                                model.CategoryId = 1;
                                model.Image = null;

                                ModelState.Clear();
                                successMessage = "TypeOfFurniture Created Successfully !";

                                ViewBag.Message = "TypeOfFurniture Created Successfully !";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                errorMessage = response.IsSuccessStatusCode.ToString();

                                ViewBag.Message = response.ReasonPhrase;
                            }

                        }
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Please provide all the required fields";

                    }
                }
                else
                {
                    ModelState.AddModelError("TypeOfFurnitureForCreationDto.CategoryId", "The CategoryId is required");

                }

            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
            }
            return View();
        }


    }
}
