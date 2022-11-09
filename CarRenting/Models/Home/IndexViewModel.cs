using CarRenting.Services.Cars.Models;

namespace CarRenting.Models.Home
{
    public class IndexViewModel
    {
        public int TotalCars { get; set; }

        public int TotalUsers { get; set; }

        public int TotalRents { get; set; }

        public List<LatestCarServiceModel> Cars { get; set; }
    }
}
