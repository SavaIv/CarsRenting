using AutoMapper;
using CarRenting.Controllers;
using CarRenting.Data.Models;
using CarRenting.Models.Home;
using CarRenting.Services.Cars;
using CarRenting.Services.Statistics;
using CarRenting.Test.Mocks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using Xunit;

namespace CarRenting.Test.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            // Arrange
            var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            var cars = Enumerable
                .Range(1, 10)
                .Select(i => new Car
                {   Brand = "brand",
                    Description = "description",
                    ImageUrl = "imageUrl",
                    Model = "model"
                });

            data.Cars.AddRange(cars);
            data.Users.Add(new User());

            data.SaveChanges();

            var carService = new CarService(data, mapper);
            var statisticsService = new StatisticsService(data);

            var homeController = new HomeController(statisticsService, carService);

            // Act
            var result = homeController.Index();

            // Assert
            Assert.NotNull(result);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = viewResult.Model;

            var indexModel = Assert.IsType<IndexViewModel>(model);
            
            Assert.Equal(3, indexModel.Cars.Count);
            Assert.Equal(10, indexModel.TotalCars);
            Assert.Equal(1, indexModel.TotalUsers);
        }


        [Fact]
        public void ErrorShouldReturnView()
        {
            // Arrange
            var homeController = new HomeController(null, null);

            // Act
            var result = homeController.Error();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
