using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarRenting.Infrastructure.ClaimsPrincipalExtensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using CarRenting.Models;
using CarRenting.Services.Cars;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly CarRentingDbContext data;

        public CarsController(ICarService _cars, CarRentingDbContext _data)
        {
            cars = _cars;
            data = _data;
        }

        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult = this.cars.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);

            var carBrands = this.cars.AllCarBrands();

            query.Brands = carBrands;
            query.TotalCars = queryResult.TotalCars;      
            query.Cars = queryResult.Cars;                            

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!UserIsDealer())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new AddCarFormModel
            {
                Categories = GetCarCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddCarFormModel car)
        {
            var dealerId = data
                .Dealers
                .Where(d => d.UserId == User.GetId())
                .Select(d => d.Id)
                .FirstOrDefault();

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

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
                CategoryId = car.CategoryId,
                DealerId = dealerId
            };

            data.Cars.Add(newCar);
            data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private bool UserIsDealer()
        {
            return data
                .Dealers
                .Any(d => d.UserId == User.GetId());
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
