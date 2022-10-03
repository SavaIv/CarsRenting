using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add() => View(new AddCarFormModel
        {
            Categories = GetCarCategories()
        });

        [HttpPost]
        public IActionResult Add(AddCarFormModel car)
        {
            if (!data.Categories.Any(c => c.Id == car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = GetCarCategories();

                return View(car);
            }

            var newCar = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                Year = car.Year,
                CategoryId = car.CategoryId
            };

            data.Cars.Add(newCar);
            data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<CarCategoryViewModel> GetCarCategories()
        {
            return data.Categories
                .Select(c => new CarCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}
