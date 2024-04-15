using System.Net.Http.Headers;
using System.Text;
using Event_Burst_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Event_Burst_Web_App.Controllers;

public class SpeakerController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SpeakerController> _logger;

    public SpeakerController(ILogger<SpeakerController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://localhost:8004/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/shiny-barnacle/speaker/get-all");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Speaker>>>(responseData);

            if (apiResponse.Message == "Success")
            {
                var speakers = apiResponse.Data;
                return View(speakers);
            }

            _logger.LogError("Failed to fetch speakers: {message}", apiResponse.Message);
            return StatusCode(500); // Return server error status code
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling API");
            return StatusCode(500); // Return server error status code
        }
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Speaker speaker)
    {
        try
        {
            var json = JsonConvert.SerializeObject(speaker);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/shiny-barnacle/speaker/create", content);

            response.EnsureSuccessStatusCode(); // Throw on error response

            return RedirectToAction("Index"); // Redirect to index or another page on success
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling API");
            return StatusCode(500); // Return server error status code
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            Console.WriteLine(id);
            var response = await _httpClient.DeleteAsync($"/api/shiny-barnacle/speaker/delete/{id}");
            response.EnsureSuccessStatusCode(); // Throw on error response

            return RedirectToAction("Index"); // Redirect to index after successful deletion
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling API");
            return StatusCode(500); // Return server error status code
        }
    }
}