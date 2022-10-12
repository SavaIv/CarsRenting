using System.ComponentModel.DataAnnotations;

using static CarRenting.Data.DataConstants.Category;

namespace CarRenting.Data.Models
{
    public class Category
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        public IEnumerable<Car> Cars { get; init;} = new List<Car>();
    }
}
