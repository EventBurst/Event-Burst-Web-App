using Microsoft.AspNetCore.Mvc;

namespace Event_Burst_Web_App.Controllers;

public class OrganizerController : Controller
{
    private readonly ILogger<EventController> _logger;

    public OrganizerController(ILogger<EventController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}