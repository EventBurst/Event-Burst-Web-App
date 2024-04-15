using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Event_Burst_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Microsoft.Extensions.Http;

namespace Event_Burst_Web_App.Controllers
{
    public class SponsorController : Controller
    {
        private readonly ILogger<SponsorController> _logger;
        private readonly HttpClient _httpClient;

        public SponsorController(ILogger<SponsorController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8004"); 
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/shiny-barnacle/sponsor/get-all");
                response.EnsureSuccessStatusCode(); // Throw on error response
                
                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Sponsor>>>(responseData);

                Console.WriteLine(apiResponse.Data[0].Name);

                if (apiResponse.Message == "Success")
                {
                    var sponsors = apiResponse.Data;
                    return View(sponsors);
                }
                else
                {
                    _logger.LogError("Failed to fetch sponsors: {message}", apiResponse.Message);
                    return StatusCode(500); // Return server error status code
                }
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
        
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                // Fetch the sponsor details by ID
                var response = await _httpClient.GetAsync($"/api/shiny-barnacle/sponsor/get/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error response

                var responseData = await response.Content.ReadAsStringAsync();
                var sponsor = JsonConvert.DeserializeObject<Sponsor>(responseData);

                return View(sponsor); // Return the view with the sponsor details for editing
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling API");
                return StatusCode(500); // Return server error status code
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Sponsor sponsor)
        {
            try
            {
                var json = JsonConvert.SerializeObject(sponsor);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/shiny-barnacle/sponsor/create", content);

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
                var response = await _httpClient.DeleteAsync($"/api/shiny-barnacle/sponsor/delete/{id}");
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
