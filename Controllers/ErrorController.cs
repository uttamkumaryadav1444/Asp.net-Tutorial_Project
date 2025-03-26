using Microsoft.AspNetCore.Mvc;
using EventManagementWebApp.ViewModels;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        var errorViewModel = new ErrorViewModel
        {
            StatusCode = statusCode,
            Message = statusCode switch
            {
                404 => "Resource not found.",
                401 => "Unauthorized access.",
                500 => "An internal server error occurred.",
                _ => "An unexpected error occurred."
            }
        };

        return View("Error", errorViewModel); 
    }
}
