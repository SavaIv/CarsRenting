using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Areas.Admin.Controllers
{
    public class CarsController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
