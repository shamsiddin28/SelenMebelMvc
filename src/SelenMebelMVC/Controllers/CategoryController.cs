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

        private string baseURL = "https://selenmebelapi20240307024627.azurewebsites.net/api/";


        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task DownloadAndSaveImageAsync(string imageName)
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(baseURL + $"Categories/DownloadByImageName?imageName={imageName}");

                    if (response.IsSuccessStatusCode)
                    {
                        string newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImages", "Images", imageName);
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                             fileStream = new FileStream(newImagePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        TempData["InfoMessage"] = $"Failed to download image. Status code: {response.StatusCode}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchBy, string searchValue, int pageIndex = 1, int pageSize = 3)
        {
            var categories = new List<CategoryForResultDto>();
            var categoriesImages = new List<string>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync($"Categories/ByPagination?PageSize={pageSize}&PageIndex={pageIndex}");
                HttpResponseMessage getAllCategories = await client.GetAsync("Categories");
                getData.EnsureSuccessStatusCode();

                if (getData.IsSuccessStatusCode && getAllCategories.IsSuccessStatusCode)
                {
                    double totalItems = 0;
                    if (getData.Headers.TryGetValues("X-Total-Count", out var totalCountValues))
                    {
                        string totalCountString = totalCountValues?.FirstOrDefault();
                        totalItems = int.Parse(totalCountString);
                    }

                    string results = getData.Content.ReadAsStringAsync().Result;
                    categories = JsonConvert.DeserializeObject<List<CategoryForResultDto>>(results).OrderByDescending(f => f.Id).ToList();
                    foreach (var item in categories)
                    {
                        await DownloadAndSaveImageAsync(item.Image);
                    }
                    ViewBag.Categories = categories;

                    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                    ViewData["TotalItems"] = totalItems;
                    ViewData["PageIndex"] = pageIndex;
                    ViewData["PageSize"] = pageSize;
                    ViewData["TotalPages"] = totalPages;

                    if (categories.Count() == 0)
                    {
                        TempData["InfoMessage"] = "Currently Furnitures not available in the Database";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(searchValue))
                        {
                            TempData["InfoMessage"] = "Please provide the search value.";
                            return View(categories);
                        }
                        else
                        {
                            if (getAllCategories.IsSuccessStatusCode)
                            {
                                string resultCategories = getAllCategories.Content.ReadAsStringAsync().Result;
                                var allCategories = JsonConvert.DeserializeObject<List<CategoryForResultDto>>(resultCategories);
                                ViewData["SearchBy"] = searchBy;
                                ViewData["SearchValue"] = searchValue;

                                for (int i = 1; i <= totalPages; i++)
                                {
                                    var result = allCategories.Skip((i - 1) * pageSize).Take(pageSize);
                                    if (searchBy.ToLower() == "categoryname")
                                    {
                                        var searchByCategoryName = result.Where(c => c.Name.ToLower().Contains(searchValue.ToLower()));
                                        if (searchByCategoryName.Any())
                                        {
                                            ViewData["PageIndex"] = i;
                                            ViewData["PageSize"] = pageSize;

                                            return View(searchByCategoryName);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (categories != null)
                                {
                                    // Set pagination data in ViewData
                                    ViewData["PageIndex"] = pageIndex;
                                    ViewData["TotalItems"] = totalItems;
                                    ViewData["PageSize"] = pageSize;
                                    ViewData["TotalPages"] = totalPages;

                                    return View(categories);
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = getAllCategories.ReasonPhrase.ToString();
                                return View(categories);

                            }
                        }
                    }


                    // Set pagination data in ViewData
                    ViewData["PageIndex"] = pageIndex;
                    ViewData["TotalItems"] = totalItems;
                    ViewData["PageSize"] = pageSize;
                    ViewData["TotalPages"] = totalPages;


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
            else if (model.Image == null)
            {
                ModelState.AddModelError("FurnitureCreationDto.Image", "The image file is required");
            }
            else if (model.Name == null)
            {
                ModelState.AddModelError("FurnitureCreationDto.Name", "The name is required");
            }

            if (ModelState.IsValid && model.Image != null && model.Name != null)
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
                    TempData["SuccessMessage"] = $"{id}th-Id Category is Deleted Successfully !";
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

