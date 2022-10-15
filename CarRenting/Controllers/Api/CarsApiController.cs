using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models;
using CarRenting.Models.Api.Cars;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers.Api
{
    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly CarRentingDbContext data;

        public CarsApiController(CarRentingDbContext _data)
        {
            data = _data;
        }

        [HttpGet]
        public ActionResult<AllCarsApiResponseModel> All([FromQuery] AllCarsApiRequestModel query)
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
                .Skip((query.CurrentPage - 1) * query.CarsPerPage)
                .Take(query.CarsPerPage)
                .Select(c => new CarResponseModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();

            ;
            return new AllCarsApiResponseModel
            {
                TotalCars = totalCars,
                CarsPerPage = query.CarsPerPage,
                CurrentPage = query.CurrentPage,
                Cars = cars
            };
        }

        //[HttpGet]
        //public IEnumerable<Car> GetCars()
        //{
        //    return data.Cars.ToList();
        //}

        //[HttpGet]
        //[Route("{id}")]
        //public IActionResult GetDetails(int id)
        //{
        //    var car = data.Cars.Find(id);

        //    if (car == null)
        //    {
        //        return NotFound();

        //    }

        //    return Ok(car);
        //}

        //public IActionResult SaveCar(Car car)
        //{
        //    return Ok();
        //}
    }
}
