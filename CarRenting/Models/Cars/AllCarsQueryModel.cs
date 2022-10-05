using System.ComponentModel.DataAnnotations;

namespace CarRenting.Models.Cars
{
    public class AllCarsQueryModel
    {
        public string Brand { get; set; }

        public IEnumerable<string> Brands { get; set; }

        [Display(Name = "Search by text:")]
        public string SearchTerm { get; set; }

        public CarSorting Sorting { get; set; }

        public IEnumerable<CarListingViewModel> Cars { get; set; }
    }
}
