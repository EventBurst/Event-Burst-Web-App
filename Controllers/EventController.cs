using System;
using System.Net;
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

            // Create HttpClient with HttpClientHandler to manage cookies
            var httpClientHandler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            _httpClient = new HttpClient(httpClientHandler);
            _httpClient.BaseAddress = new Uri($"http://localhost:8004/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Ensure HttpContext is available
                var httpContext = HttpContext;

                // Get cookies from the current HTTP context
                var cookies = httpContext.Request.Cookies;

                // Attach cookies to the HttpClient's DefaultRequestHeaders
                foreach (var cookie in cookies)
                {
                    _httpClient.DefaultRequestHeaders.Add("Cookie", $"{cookie.Key}={cookie.Value}");
                }
                var response = await _httpClient.GetAsync("/api/shiny-barnacle/event/get-organizer-events");
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

       

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                Console.WriteLine(id);

                // Ensure HttpContext is available
                var httpContext = HttpContext;

                // Get cookies from the current HTTP context
                var cookies = httpContext.Request.Cookies;

                // Attach cookies to the HttpClient's DefaultRequestHeaders
                foreach (var cookie in cookies)
                {
                    _httpClient.DefaultRequestHeaders.Add("Cookie", $"{cookie.Key}={cookie.Value}");
                }

                // Send DELETE request
                var response = await _httpClient.DeleteAsync($"/api/shiny-barnacle/event/delete/{id}");
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
}
