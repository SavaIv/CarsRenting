using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CarRenting.Infrastructure.ClaimsPrincipalExtensions;
using static CarRenting.WebConstants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using CarRenting.Models;
using CarRenting.Services.Cars;
using CarRenting.Services.Dealers;
using AutoMapper;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IDealerService dealers;
        private readonly IMapper mapper;
        
        public CarsController(
            ICarService _cars,
            IDealerService _dealers,
            IMapper _mapper)
        {
            cars = _cars;
            dealers = _dealers;
            mapper = _mapper;
        }

        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var queryResult = this.cars.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllCarsQueryModel.CarsPerPage);

            var carBrands = this.cars.AllBrands();

            query.Brands = carBrands;
            query.TotalCars = queryResult.TotalCars;
            query.Cars = queryResult.Cars;

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!dealers.IsDealer(User.Id()))
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            return View(new CarFormModel
            {
                Categories = cars.AllCategories()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(CarFormModel car)
        {
            var dealerId = dealers.IdByUser(User.Id());

            if (dealerId == 0)
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!cars.CategoryExist(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = cars.AllCategories();

                return View(car);
            }

            cars.Create(
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId,
                dealerId);

            TempData[GlobalMessageKey] = "Your car was saved successfuly";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Mine()
        {
            var myCars = cars.ByUser(User.Id());

            return View(myCars);
        }

        public IActionResult Details(int Id)
        {
            var car = this.cars.Details(Id);            

            return View(car);
        }

        [Authorize]
        public IActionResult Edit(int Id)
        {
            var userId = User.Id();

            if (!dealers.IsDealer(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            var car = cars.Details(Id);

            if (car.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var carForm = mapper.Map<CarFormModel>(car);
            carForm.Categories = cars.AllCategories();

            return View(carForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int Id, CarFormModel car)
        {
            var dealerId = dealers.IdByUser(User.Id());

            if (dealerId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(DealersController.Become), "Dealers");
            }

            if (!cars.CategoryExist(car.CategoryId))
            {
                ModelState.AddModelError(nameof(car.CategoryId), "Category does not exists");
            }

            if (!ModelState.IsValid)
            {
                car.Categories = cars.AllCategories();

                return View(car);
            }

            if (!cars.IsByDealer(Id, dealerId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            cars.Edit(
                Id,
                car.Brand,
                car.Model,
                car.Description,
                car.ImageUrl,
                car.Year,
                car.CategoryId);

            return RedirectToAction(nameof(All));
        }
    }
}
