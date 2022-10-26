using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRenting.Data;
using CarRenting.Models;
using CarRenting.Models.Home;
using CarRenting.Services.Statistics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {        
        private readonly IStatisticsService statistics;
        private readonly CarRentingDbContext data;
        private readonly IMapper mapper;

        public HomeController(
            IStatisticsService _statistics,
            CarRentingDbContext _data,
            IMapper _mapper)
        {
            statistics = _statistics;
            data = _data;
            mapper = _mapper;
        }

        public IActionResult Index()
        {            
            var cars = this.data
                .Cars
                .OrderByDescending(c => c.Id)
                .ProjectTo<CarIndexViewModel>(mapper.ConfigurationProvider)
                .Take(3)
                .ToList();

            var totalStatistics = statistics.Total();

            return View(new IndexViewModel
            {
                TotalCars = totalStatistics.TotalCars,
                TotalUsers = totalStatistics.TotalUsers,
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
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            return View();
        }
    }
}