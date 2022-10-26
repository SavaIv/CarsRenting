using CarRenting.Data;
using CarRenting.Data.Models;
using CarRenting.Services.Dealers;
using CarRenting.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarRenting.Test.Services
{
    public class DealerServiceTest
    {
        private const string UserId = "TestUserId";

        [Fact]
        public void IsDealerShouldReturnTrueWhenUserIsDealer()
        {
            // Arrange           
            var dealerservice = GetDealerService();

            // Act
            var result = dealerservice.IsDealer(UserId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDealerShouldReturnFalseWhenUserIsNotDealer()
        {
            // Arrange
            var dealerservice = GetDealerService();

            // Act
            var result = dealerservice.IsDealer("AnotherUserId");

            //Assert
            Assert.False(result);
        }

        private static IDealerService GetDealerService()
        {            
            var data = DatabaseMock.Instance;

            data.Dealers.Add(new Dealer
            {
                UserId = UserId,
                Name = "aName",
                PhoneNumber = "aPhoneNumber"
            });
            data.SaveChanges();

            return new DealerService(data);
        }
    }
}
