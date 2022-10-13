using CarRenting.Data;
using CarRenting.Models.Api.Satistics;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly CarRentingDbContext data;

        public StatisticsApiController(CarRentingDbContext _data)
        {
            data = _data;
        }

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var totalCars = data.Cars.Count();
            var totalUsers = data.Users.Count();

            var statistics = new StatisticsResponseModel
            {
                TotalCars = totalCars,
                TotalUsers = totalUsers,
                TotalRents = 0
            };

            return statistics;
        }
    }
}
