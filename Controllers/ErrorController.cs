using Event_Burst_Web_App.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Event_Burst_Web_App.Controllers;

public class ErrorController : Controller
{
    [Route("Error")]
    public IActionResult Error()
    {
        var errorViewModel = new ErrorViewModel
        {
            ErrorMessage = "An error occurred while processing your request.",
            ErrorDetails = HttpContext.Features.Get<IExceptionHandlerFeature>().Error.Message
        };

        return View(errorViewModel);
    }
}