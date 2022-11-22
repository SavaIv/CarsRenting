namespace CarRenting.Services.Cars.Models
{
    public class CarDetailsServiceModel : CarServiceModel
    {
        public string Description { get; set; }

        public string CategoryName { get; set; }

        public int DealerId { get; set; }

        public string DealerName { get; set; }

        public int CategoryId { get; set; }

        public string UserId { get; set; }
    }
}
