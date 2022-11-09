using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Models;
using CarRenting.Services.Cars.Models;

namespace CarRenting.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly CarRentingDbContext data;
        private readonly IMapper mapper;

        public CarService(CarRentingDbContext _data, IMapper _mapper)
        {
            data = _data;
            mapper = _mapper;
        }

        public CarQueryServiceModel All(
            string brand,
            string searchTerm,
            CarSorting sorting,
            int currentPage,
            int carsPerPage)
        {
            var carsQuery = this.data.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                carsQuery = carsQuery.Where(c => c.Brand == brand);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                carsQuery = carsQuery.Where(c =>
                    c.Brand.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Model.ToLower().Contains(searchTerm.ToLower()) ||
                    c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            carsQuery = sorting switch
            {
                CarSorting.DateCreated => carsQuery.OrderByDescending(c => c.Id),
                CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
                CarSorting.BrandAndModel => carsQuery.OrderBy(c => c.Brand).ThenBy(c => c.Model),
                _ => carsQuery.OrderByDescending(c => c.Id)
            };

            var totalCars = carsQuery.Count();

            var cars = GetCars(carsQuery
                .Skip((currentPage - 1) * carsPerPage)
                .Take(carsPerPage));

            return new CarQueryServiceModel
            {
                TotalCars = totalCars,
                CurrentPage = currentPage,
                CarsPerPage = carsPerPage,
                Cars = cars
            };
        }

        public CarDetailsServiceModel Details(int id)
        {
            return data
                .Cars
                .Where(c => c.Id == id)
                .ProjectTo<CarDetailsServiceModel>(mapper.ConfigurationProvider)                
                .FirstOrDefault();
        }

        public int Create(string brand, string model, string description, string imageUrl, int year, int categoryId, int dealerId)
        {
            var carData = new Car
            {
                Brand = brand,
                Model = model,
                Description = description,
                ImageUrl = imageUrl,
                Year = year,
                CategoryId = categoryId,
                DealerId = dealerId
            };

            data.Cars.Add(carData);
            data.SaveChanges();

            return carData.Id;  // <-- this is the ID from database
        }

        public bool Edit(int id, string brand, string model, string description, string imageUrl, int year, int categoryId)
        {
            var carData = data.Cars.Find(id);

            if (carData == null)
            {
                return false;
            }

            carData.Brand = brand;
            carData.Model = model;
            carData.Description = description;
            carData.ImageUrl = imageUrl;
            carData.Year = year;
            carData.CategoryId = categoryId;

            data.SaveChanges();

            return true;
        }

        public IEnumerable<CarServiceModel> ByUser(string userId)
        {
            return GetCars(data.Cars.Where(c => c.Dealer.UserId == userId));
        }

        public bool IsByDealer(int carId, int dealerId)
        {
            return data.Cars.Any(c => c.Id == carId && c.DealerId == dealerId);
        }

        public IEnumerable<string> AllBrands()
        {
            return data
                .Cars
                .Select(c => c.Brand)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        public IEnumerable<CarCategoryServiceModel> AllCategories()
        {
            return data.Categories
                .Select(c => new CarCategoryServiceModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        public bool CategoryExist(int categoryId)
        {
            return data
                .Categories
                .Any(c => c.Id == categoryId);
        }

        private static IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carQuery)
        {
            return carQuery
                .Select(c => new CarServiceModel
                {
                    Id = c.Id,
                    Brand = c.Brand,
                    Model = c.Model,
                    Year = c.Year,
                    ImageUrl = c.ImageUrl,
                    CategoryName = c.Category.Name
                })
                .ToList();
        }

        public IEnumerable<LatestCarServiceModel> Latest()
        {
            return data
                .Cars
                .OrderByDescending(c => c.Id)
                .ProjectTo<LatestCarServiceModel>(mapper.ConfigurationProvider)
                .Take(3)
                .ToList();
        }
    }
}
