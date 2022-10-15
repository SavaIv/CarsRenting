using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarRenting.Infrastructure.ClaimsPrincipalExtensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using CarRenting.Models;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarRentingDbContext data;

        public CarsController(CarRentingDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery]AllCarsQueryModel query)
        {
            var carsQuery = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Brand))
            {
                carsQuery = carsQuery.Where(c => c.Brand == query.Brand);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                    c.Brand.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    c.Model.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    c.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            carsQuery = query.Sorting switch
            {
                CarSorting.DateCreated => carsQuery.OrderByDescending(c => c.Id),
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.BrandAndModel => carsQuery.OrderBy(c => c.Brand).ThenBy(c => c.Model),
                _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars = carsQuery
                .Skip((query.CurrentPage - 1) * AllCarsQueryModel.CarsPerPage)
                .Take(AllCarsQueryModel.CarsPerPage)
                .Select(c => new CarListingViewModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();

            var carBrands = data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            query.TotalCars = totalCars;
            query.Brands = carBrands;
            query.Cars = cars;

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
