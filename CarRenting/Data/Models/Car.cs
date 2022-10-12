using System.ComponentModel.DataAnnotations;

using static CarRenting.Data.DataConstants.Car;

namespace CarRenting.Data.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CarBrandMaxLength)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(CarModelMaxLenght)]
        public string Model { get; set; }

        [Required]
        [MaxLength(CarDescriptionMaxlength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int Year { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }

        public int DealerId { get; set; }

        public Dealer Dealer { get; set; }
    }
}
