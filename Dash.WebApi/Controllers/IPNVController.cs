using Microsoft.AspNetCore.Mvc;

namespace anyhelp.Api.Controllers
{
    public class IPNVController : Controller
    {
        public IActionResult Index()
        {
            return View("index");
        }
    }
}
