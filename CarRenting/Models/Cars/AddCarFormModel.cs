using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using static CarRenting.Data.DataConstants;

namespace CarRenting.Models.Cars
{
    public class AddCarFormModel
    {
        [Required]
        [StringLength(
            CarBrandMaxLength, 
            MinimumLength = CarBrandMinLength, 
            ErrorMessage = "The field {0} reqired minimim {2} and max {1} characters")]        
        public string Brand { get; set; }

        [Required]
        [StringLength(
            CarModelMaxLenght,
            MinimumLength = CarModelMinLenght,
            ErrorMessage = "The field {0} reqired minimim {2} and max {1} characters")]        
        public string Model { get; set; }

        [Required]
        [StringLength(
            CarDescriptionMaxlength,
            MinimumLength = CarDescriptionMinlength,
            ErrorMessage = "The field {0} reqired minimim {2} and max {1} characters")]
        public string Description { get; set; }

        [Display(Name = "Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Range(CarYearMinValue, CarYearMaxValue)]
        public int Year { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [BindNever]
        public IEnumerable<CarCategoryViewModel>? Categories { get; set; }
    }
}
