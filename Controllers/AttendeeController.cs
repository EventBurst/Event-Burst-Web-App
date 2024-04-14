using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Event_Burst_Web_App.Models;

namespace Event_Burst_Web_App.Controllers;

public class EventController : Controller
{
    private readonly ILogger<EventController> _logger;

    public EventController(ILogger<EventController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}