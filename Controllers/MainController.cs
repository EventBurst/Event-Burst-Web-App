using Microsoft.AspNetCore.Mvc;

namespace Event_Burst_Web_App.Controllers;

public class MainController : Controller
{
    private readonly ILogger<EventController> _logger;

    public MainController(ILogger<EventController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}