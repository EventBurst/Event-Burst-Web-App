using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class SponsorController : Controller
{
    private readonly HttpClient _httpClient;

    public SponsorController()
    {
        _httpClient = new HttpClient();
    }

    public async Task<ActionResult> Index()
{
    var response = await _httpClient.GetAsync("http://localhost:8002/api/shiny-barnacle/sponsor/get-all");

    if (response.IsSuccessStatusCode)
    {
        
        var json = await response.Content.ReadAsStringAsync();
        var sponsorResponse = JsonConvert.DeserializeObject<SponsorResponse>(json);
        return View(sponsorResponse.Data);
    }

    // Handle error cases
    return Content("Failed to fetch sponsors.");
}

}
