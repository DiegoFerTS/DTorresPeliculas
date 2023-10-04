using Microsoft.AspNetCore.Mvc;

namespace SLWebApi.Controllers
{
    public class CineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
