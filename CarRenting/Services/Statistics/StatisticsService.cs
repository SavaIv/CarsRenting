using CarRenting.Data;

namespace CarRenting.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly CarRentingDbContext data;

        public StatisticsService(CarRentingDbContext _data)
        {
            data = _data;
        }

        public StatisticsServiceModel Total()
        {
            var totalCars = data.Cars.Count();
            var totalUsers = data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalCars = totalCars,
                TotalUsers = totalUsers
            };
        }
    }
}
