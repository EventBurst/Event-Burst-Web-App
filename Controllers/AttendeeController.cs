using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Event_Burst_Web_App.Controllers;

public class AttendeeController : Controller
{
        private readonly HttpClient _httpClient;

        public AttendeeController()
        {
           
            _httpClient = new HttpClient();
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
    return Content("Failed to fetch sponsors.");
}
public async Task<IActionResult> BookedEvents()
{
//    var response = await _httpClient.GetAsync("http://localhost:8003/api/legendary-octo-events/attendee/get-attendee-tickets");
    var response = await _httpClient.GetAsync("http://localhost:8004/api/shiny-barnacle/event/get-all");
    Console.WriteLine(response);
    if (response.IsSuccessStatusCode)
    {
        
        var json = await response.Content.ReadAsStringAsync();
        var sponsorResponse = JsonConvert.DeserializeObject<EventData>(json);
        return View(sponsorResponse.Data);
    }

    // Handle error cases
    return Content("Failed to fetch sponsors.");
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