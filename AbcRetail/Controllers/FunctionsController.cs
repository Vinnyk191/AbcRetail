using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace AbcRetail.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly HttpClient _httpClient;

        public FunctionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:7071"); // Local Azure Functions host
        }

        // GET: /Functions
        public IActionResult Index(string message = null, bool success = true)
        {
            ViewBag.Message = message;
            ViewBag.Success = success;
            return View();
        }

        // Call StoreToTable function
        public async Task<IActionResult> StoreToTable()
        {
            var response = await _httpClient.GetAsync("/api/StoreToTable");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", new { message = "Data stored into Azure Table successfully!", success = true });

            return RedirectToAction("Index", new { message = "Failed to store data into Azure Table.", success = false });
        }

        // Call UploadToBlob function
        public async Task<IActionResult> UploadToBlob()
        {
            var response = await _httpClient.GetAsync("/api/UploadToBlob");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", new { message = "File uploaded to Blob Storage successfully!", success = true });

            return RedirectToAction("Index", new { message = "Failed to upload file to Blob Storage.", success = false });
        }

        // Call EnqueueTransaction function
        public async Task<IActionResult> EnqueueTransaction()
        {
            var response = await _httpClient.GetAsync("/api/EnqueueTransaction");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", new { message = "Transaction added to Queue successfully!", success = true });

            return RedirectToAction("Index", new { message = "Failed to enqueue transaction.", success = false });
        }

        // Call WriteToFileShare function
        public async Task<IActionResult> WriteToFileShare()
        {
            var response = await _httpClient.GetAsync("/api/WriteToFileShare");
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", new { message = "Log written to File Share successfully!", success = true });

            return RedirectToAction("Index", new { message = "Failed to write log to File Share.", success = false });
        }
    }
}
