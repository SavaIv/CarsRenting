using CarRenting.Data;
using CarRenting.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService _statistics)
        {
            statistics = _statistics;
        }

        [HttpGet]
        public StatisticsServiceModel GetStatistics()
        {
           return statistics.Total();
        }
    }
}
