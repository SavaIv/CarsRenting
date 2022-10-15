namespace CarRenting.Models.Api.Cars
{
    public class AllCarsApiRequestModel
    {
        public string Brand { get; set; } = "";

        public string SearchTerm { get; set; } = "";

        public CarSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int CarsPerPage { get; set; } = 10;

        public int TotalCars { get; set; }
    }
}
