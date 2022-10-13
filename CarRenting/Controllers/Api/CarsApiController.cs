using CarRenting.Data;
using CarRenting.Data.Models;
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
        public IEnumerable<Car> GetCars()
        {
            return data.Cars.ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDetails(int id)
        {
            var car = data.Cars.Find(id);

            if (car == null)
            {
                return NotFound();

            }

            return Ok(car);
        }

        public IActionResult SaveCar(Car car)
        {
            return Ok();
        }
    }
}
