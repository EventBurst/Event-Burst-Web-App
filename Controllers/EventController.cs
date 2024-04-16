using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Event_Burst_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Event_Burst_Web_App.Controllers
{
    public class EventController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EventController> _logger;

        public EventController(ILogger<EventController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri($"http://localhost:8004/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/shiny-barnacle/event/get-all");
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Event>>>(responseData);

                if (apiResponse.Success == true)
                {
                    var events = apiResponse.Data;
                    return View(events);
                }

                _logger.LogError("Failed to fetch events: {message}", apiResponse.Message);
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

        // [HttpPost]
        // public async Task<IActionResult> Create(Event event)
        // {
        //     try
        //     {
        //         var json = JsonConvert.SerializeObject(event);
        //         var content = new StringContent(json, Encoding.UTF8, "application/json");

        //         var response = await _httpClient.PostAsync("/api/shiny-barnacle/event/create", content);

        //         response.EnsureSuccessStatusCode(); // Throw on error response

        //         return RedirectToAction("Index"); // Redirect to index or another page on success
        //     }
        //     catch (HttpRequestException ex)
        //     {
        //         _logger.LogError(ex, "Error calling API");
        //         return StatusCode(500); // Return server error status code
        //     }
        // }

        // [HttpPost]
        // public async Task<IActionResult> Delete(string id)
        // {
        //     try
        //     {
        //         Console.WriteLine(id);
        //         var response = await _httpClient.DeleteAsync($"/api/shiny-barnacle/event/delete/{id}");
        //         response.EnsureSuccessStatusCode(); // Throw on error response

        //         return RedirectToAction("Index"); // Redirect to index after successful deletion
        //     }
        //     catch (HttpRequestException ex)
        //     {
        //         _logger.LogError(ex, "Error calling API");
        //         return StatusCode(500); // Return server error status code
        //     }
        // }

        // [HttpGet]
        // public async Task<IActionResult> Update(string id)
        // {
        //     try
        //     {
        //         // Fetch the event details by ID
        //         var response = await _httpClient.GetAsync($"/api/shiny-barnacle/event/get/{id}");
        //         response.EnsureSuccessStatusCode(); // Throw on error response

        //         var responseData = await response.Content.ReadAsStringAsync();
        //         var event = JsonConvert.DeserializeObject<ApiResponse<Event>>(responseData);

        //         if (event.Message == "Success")
        //         {
        //             // Pass the event details to the view
        //             var eventDetail = event.Data;
        //             return View(eventDetail);
        //         }

        //         _logger.LogError("Failed to fetch events: {message}", event.Message);
        //         return StatusCode(500); // Return server error status code
        //     }
        //     catch (HttpRequestException ex)
        //     {
        //         _logger.LogError(ex, "Error calling API");
        //         return StatusCode(500); // Return server error status code
        //     }
        // }

        // [HttpPost]
        // public async Task<IActionResult> UpdateEvent(Event event)
        // {
        //     try
        //     {
        //         var json = JsonConvert.SerializeObject(event);
        //         var content = new StringContent(json, Encoding.UTF8, "application/json");

        //         var response = await _httpClient.PutAsync($"/api/shiny-barnacle/event/update/{event._id}", content);

        //         response.EnsureSuccessStatusCode(); // Throw on error response

        //         return RedirectToAction("Index"); // Redirect to index or another page on success
        //     }
        //     catch (HttpRequestException ex)
        //     {
        //         _logger.LogError(ex, "Error calling API");
        //         return StatusCode(500); // Return server error status code
        //     }
        // }
    }
}
