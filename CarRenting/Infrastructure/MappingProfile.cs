using AutoMapper;
using CarRenting.Data.Models;
using CarRenting.Models.Cars;
using CarRenting.Models.Home;
using CarRenting.Services.Cars.Models;

namespace CarRenting.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, LatestCarServiceModel>();
            CreateMap<CarDetailsServiceModel, CarFormModel>();

            CreateMap<Car, CarDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Dealer.UserId))
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));
        }
    }
}
