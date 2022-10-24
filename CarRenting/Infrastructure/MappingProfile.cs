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
            CreateMap<Car, CarIndexViewModel>();
            CreateMap<CarDetailsServiceModel, CarFormModel>();

            CreateMap<Car, CarDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Dealer.UserId));
        }
    }
}
