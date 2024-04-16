using System.Net.Http.Headers;
using System.Text;
using Event_Burst_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Event_Burst_Web_App.Controllers;

public class AgendaController: Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AgendaController> _logger;

    public AgendaController(ILogger<AgendaController> logger, IHttpClientFactory httpClientFactory)
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
            var response = await _httpClient.GetAsync("/api/shiny-barnacle/agenda/get-all");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<Agenda>>>(responseData, jsonSettings);
            
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
    public async Task<IActionResult> Create(Agenda agenda)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(agenda);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/shiny-barnacle/agenda/create", content);
            
                if (response.IsSuccessStatusCode)
                {
                    // Agenda created successfully, redirect to index
                    return RedirectToAction("Index");
                }
                else
                {
                    // Handle API error responses
                    _logger.LogError("Error creating agenda: {statusCode}", response.StatusCode);
                    return StatusCode((int)response.StatusCode);
                }
            }
            else
            {
                // Invalid model state, return to the create view with validation errors
                return View(agenda);
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
            Console.WriteLine(id);
            var response = await _httpClient.DeleteAsync($"/api/shiny-barnacle/agenda/delete/{id}");
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
            // Fetch the sponsor details by ID
            var response = await _httpClient.GetAsync($"/api/shiny-barnacle/agenda/get/{id}");
            response.EnsureSuccessStatusCode(); // Throw on error response

            var responseData = await response.Content.ReadAsStringAsync();
            var agenda = JsonConvert.DeserializeObject<ApiResponse<Agenda>>(responseData);
            
            if (agenda.Message == "Success")
            {
                // Pass the speaker details to the view
                var agendaData= agenda.Data;
                return View(agendaData); 
            }
                
            _logger.LogError("Failed to fetch sponsors: {message}", agenda.Message);
            return StatusCode(500); // Return server error status code
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling API");
            return StatusCode(500); // Return server error status code
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAgenda(Agenda agenda)
    {
        try
        {
            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var json = JsonConvert.SerializeObject(agenda, jsonSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"/api/shiny-barnacle/agenda/update/{agenda.AgendaId}", content);
            
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