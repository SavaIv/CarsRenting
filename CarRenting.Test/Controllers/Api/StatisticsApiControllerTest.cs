using CarRenting.Controllers.Api;
using CarRenting.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarRenting.Test.Controllers.Api
{
    public class StatisticsApiControllerTest
    {
        [Fact]
        public void GetStatisticsShouldReturnTotalStatistics()
        {
            // Arrange
            var statisticsApiController = new StatisticsApiController(StatisticsServiceMock.Instance);

            //Act
            var result = statisticsApiController.GetStatistics();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.TotalCars);
            Assert.Equal(10, result.TotalRents);
            Assert.Equal(15, result.TotalUsers);
        }
    }
}
