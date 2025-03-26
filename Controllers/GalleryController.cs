using Microsoft.AspNetCore.Mvc;

namespace EventManagementWebApp.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Gallery()
        {
            return View();
        }
    }
}
