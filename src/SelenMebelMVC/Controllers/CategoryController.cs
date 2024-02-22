using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelenMebel.Service.DTOs.Categories;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace SelenMebelMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private string baseURL = "https://localhost:7200/api/";
        
        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
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
                    return View(categories);
                }
                else
                {
                    TempData["ErrorMessage"] = getData.ReasonPhrase;
					return View(null);
                }

            }

        }

        [HttpGet]
        public async Task<ViewResult> Edit(long id)
        {

            using (var client = new HttpClient())
            {
                CategoryForResultDto model = new CategoryForResultDto(); 
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync($"Categories/{id}");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<CategoryForResultDto>(results);
                     
                    return View("Edit", model);
                }
                else
                {
                    return View("Index");
                }

            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(long id, CategoryForUpdateDto model)
        {
            if (id < 0)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var apiClient = _httpClientFactory.CreateClient("client");
                var apiUrl = apiClient.BaseAddress + $"api/Categories/{id}";

                using (var multipartContent = new MultipartFormDataContent())
                {

                    multipartContent.Add(new StringContent(model.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
                    var imageContent = new StreamContent(model.Image.OpenReadStream());
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                    multipartContent.Add(imageContent, "Image", model.Image.FileName);

                    var response = await apiClient.PutAsync(apiUrl, multipartContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        TempData["SuccessMessage"] = "Category Updated Successfully !";

                        model.Name = "";
                        model.Image = null;

                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                    else
                    {
						TempData["ErrorMessage"] = response.ReasonPhrase;
                        return View("Edit", model);
                    }

                }
            }
            else
            {
				TempData["InfoMessage"] = "Please provide all the required fields";
                return View("Edit", model);

            }

        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryForCreationDto model)
        {
            try
            {
                if (model.Name != null)
                {
                    if (model.Image == null)
                    {
                        ModelState.AddModelError("FurnitureCreationDto.Image", "The image file is required");
                    }

                    if (ModelState.IsValid)
                    {
                        var apiClient = _httpClientFactory.CreateClient("client");
                        var apiUrl = apiClient.BaseAddress + "api/Categories";

                        using (var multipartContent = new MultipartFormDataContent())
                        {

                            multipartContent.Add(new StringContent(model.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), "name");
                            var imageContent = new StreamContent(model.Image.OpenReadStream());
                            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Image.Jpeg);
                            multipartContent.Add(imageContent, "Image", model.Image.FileName);

                            var response = await apiClient.PostAsync(apiUrl, multipartContent);
                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                TempData["SuccessMessage"] = "Category Created Successfully !";

                                model.Name = "";
                                model.Image = null;

                                ModelState.Clear();
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["ErrorMessage"] = response.ReasonPhrase;    
                            }

                        }
                    }
                    else
                    {
						TempData["InfoMessage"] = "Please provide all the required fields";

                    }
                }

            }
            catch (Exception ex)
            {

				TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> Delete(long id)
        {
            using (var client = new HttpClient())
            {
                CategoryForResultDto model = new CategoryForResultDto();
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync($"Categories/{id}");

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<CategoryForResultDto>(results);

                    return View("Delete", model);
                }
                else
                {
                    return View("Index");
                }

            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(long id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var apiClient = _httpClientFactory.CreateClient("client");
                var apiUrl = apiClient.BaseAddress + $"api/Categories/{id}";

                HttpResponseMessage response = apiClient.DeleteAsync(apiUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Category is Deleted Successfully !";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = response.ReasonPhrase;
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
                throw;
            }
        }
    }
}

