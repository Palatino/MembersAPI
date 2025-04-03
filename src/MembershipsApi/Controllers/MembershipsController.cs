using Microsoft.AspNetCore.Mvc;

namespace MembershipsApi.Controllers
{
    public class MembershipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
