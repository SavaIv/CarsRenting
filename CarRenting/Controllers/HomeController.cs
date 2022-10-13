using CarRenting.Data;
using CarRenting.Models;
using CarRenting.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {        
        private readonly CarRentingDbContext data;

        public HomeController(CarRentingDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var totalCars = data.Cars.Count();
            var totalUsers = data.Users.Count();

            var cars = this.data
                .Cars
                .OrderByDescending(c => c.Id)
                .Select(c => new CarIndexViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl
                })
                .Take(3)
                .ToList();

            return View(new IndexViewModel
            {
                TotalCars = totalCars,
                TotalUsers = totalUsers,
                Cars = cars
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}