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

namespace Event_Burst_Web_App.Controllers;

public class AttendeeController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EventController> _logger;

    public AttendeeController(ILogger<EventController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;

        // Create HttpClient with HttpClientHandler to manage cookies
        var httpClientHandler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        _httpClient = new HttpClient(httpClientHandler);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> ExploreEvent()
    {
        var response = await _httpClient.GetAsync("http://localhost:8004/api/shiny-barnacle/event/get-all");

        if (response.IsSuccessStatusCode)
        {

            var json = await response.Content.ReadAsStringAsync();
            var sponsorResponse = JsonConvert.DeserializeObject<EventData>(json);
            return View(sponsorResponse.Data);
        }

        // Handle error cases
        return Content("Failed to fetch events.");
    }
    public async Task<IActionResult> BookedEvents()
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
        // var response = await _httpClient.GetAsync("http://localhost:8003/api/legendary-octo-events/attendee/get-attendee-tickets");
        var response = await _httpClient.GetAsync("http://localhost:8004/api/shiny-barnacle/event/get-all");
        Console.WriteLine(response);
        if (response.IsSuccessStatusCode)
        {

            var json = await response.Content.ReadAsStringAsync();
            var sponsorResponse = JsonConvert.DeserializeObject<EventData>(json);
            return View(sponsorResponse.Data);
        }

        // Handle error cases
        return Content("Failed to fetch bookedEvents.");
    }

    public IActionResult Register()
    {
        return View();
    }
    public IActionResult Login()
    {
        return View();
    }
}