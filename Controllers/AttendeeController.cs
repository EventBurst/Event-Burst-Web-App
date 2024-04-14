using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Event_Burst_Web_App.Models;

namespace Event_Burst_Web_App.Controllers;

public class AttendeeController : Controller
{
    private readonly ILogger<AttendeeController> _logger;

    public AttendeeController(ILogger<AttendeeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult ExploreEvent()
    {
        return View();
    }
}