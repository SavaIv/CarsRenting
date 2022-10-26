using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRenting.Test.Mocks
{
    public static class MapperMock
    {
        public static IMapper Instance
        {
            get 
            {
                var mapperMock = new Mock<IMapper>();

                mapperMock
                    .SetupGet(m => m.ConfigurationProvider)
                    .Returns(Mock.Of<IConfigurationProvider>());

                return mapperMock.Object;
            }
        }
    }
}
