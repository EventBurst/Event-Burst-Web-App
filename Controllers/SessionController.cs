using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Event_Burst_Web_App.Models;

namespace Event_Burst_Web_App.Controllers
{
    public class SessionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger, IHttpClientFactory httpClientFactory)
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
                var response = await _httpClient.GetAsync("/api/shiny-barnacle/session/get-all");
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Session>>>(responseData);

                if (apiResponse.Message == "Success")
                {
                    var sessions = apiResponse.Data;
                    return View(sessions);
                }

                _logger.LogError("Failed to fetch sessions: {message}", apiResponse.Message);
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
        public async Task<IActionResult> Create(Session session)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(session);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync("/api/shiny-barnacle/session/create", content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Session created successfully, redirect to index
                        return RedirectToAction("Index");
                    }

                    // Handle API error responses
                    _logger.LogError("Error creating session: {statusCode}", response.StatusCode);
                    return StatusCode((int)response.StatusCode);
                }
                else
                {
                    // Invalid model state, return to the create view with validation errors
                    return View(session);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request exceptions
                _logger.LogError(ex, "Error calling API");
                return StatusCode(500); // Return server error status code
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/shiny-barnacle/session/delete/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error response

                return RedirectToAction("Index"); // Redirect to index after successful deletion
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling API");
                return StatusCode(500); // Return server error status code
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                // Fetch the session details by ID
                var response = await _httpClient.GetAsync($"/api/shiny-barnacle/session/get/{id}");
                response.EnsureSuccessStatusCode(); // Throw on error response

                var responseData = await response.Content.ReadAsStringAsync();
                var session = JsonConvert.DeserializeObject<ApiResponse<Session>>(responseData);

                if (session.Message == "Success")
                {
                    // Pass the session details to the view
                    var sessionData = session.Data;
                    return View(sessionData);
                }

                _logger.LogError("Failed to fetch session: {message}", session.Message);
                return StatusCode(500); // Return server error status code
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling API");
                return StatusCode(500); // Return server error status code
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSession(Session session)
        {
            try
            {
                var json = JsonConvert.SerializeObject(session);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/shiny-barnacle/session/update/{session.SessionId}", content);

                response.EnsureSuccessStatusCode(); // Throw on error response

                return RedirectToAction("Index"); // Redirect to index or another page on success
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling API");
                return StatusCode(500); // Return server error status code
            }
        }
    }
}
