namespace CarRenting.Models.Api.Cars
{
    public class AllCarsApiResponseModel
    {
        public int CurrentPage { get; set; }

        public int CarsPerPage { get; set; }

        public int TotalCars { get; set; }

        public IEnumerable<CarResponseModel> Cars { get; set; }
    }
}
