using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Infrastructure;
using CarRenting.Models.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRenting.Controllers
{
    public class DealersController : Controller
    {
        private readonly CarRentingDbContext data;

        public DealersController(CarRentingDbContext _data)
        {
            data = _data;
        }

        [Authorize]
        public IActionResult Become()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Become(BecomeDealerFormModel dealer)
        {
            var userId = User.Id();

            var userIdAlreadyDealer = data
                .Dealers
                .Any(d => d.UserId == userId);

            if (userIdAlreadyDealer)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(dealer);
            }

            var dealerData = new Dealer
            {
                Name = dealer.Name,
                PhoneNumber = dealer.PhoneNumber,
                UserId = userId
            };

            data.Dealers.Add(dealerData);
            data.SaveChanges();

            return RedirectToAction("All", "Cars");
        }
    }
}
