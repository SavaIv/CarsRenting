using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models;
using CarRenting.Models.Api.Cars;
using CarRenting.Services.Cars;
using CarRenting.Services.Cars.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers.Api
{
    [ApiController]
    [Route("api/cars")]
    public class CarsApiController : ControllerBase
    {
        private readonly ICarService cars;

        public CarsApiController(ICarService _cars)
        {
            cars = _cars;
        }

        [HttpGet]
        public CarQueryServiceModel All([FromQuery] AllCarsApiRequestModel query)
        {
            return cars.All(
                query.Brand,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.CarsPerPage);
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
