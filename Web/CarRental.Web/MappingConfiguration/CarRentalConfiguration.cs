using AutoMapper;
using CarRental.DTOs.Cars;
using CarRental.DTOs.Orders;
using CarRental.Models;
using CarRental.Web.Areas.Administration.ViewModels.Cars;
using CarRental.Web.Areas.Administration.ViewModels.Locations;
using CarRental.Web.ViewModels.Orders;

namespace CarRental.Web.MappingConfiguration
{
    public class CarRentalConfiguration : Profile
    {
        public CarRentalConfiguration()
        {
            this.CreateMap<AddLocationViewModel, Location>();
            this.CreateMap<Order, OrderDto>();
            this.CreateMap<OrderDto, MyOrdersViewModel>()
               .ForMember(dest => dest.PickUpLocation, src => src.MapFrom(x => x.PickUpLocation.Name))
               .ForMember(dest => dest.ReturnLocation, src => src.MapFrom(x => x.ReturnLocation.Name))
               .ForMember(dest => dest.CarModel, src => src.MapFrom(x => x.Car.Model)).ReverseMap();
            this.CreateMap<AddCarViewModel, Car>();
            this.CreateMap<ListCarDto, Car>();
            this.CreateMap<CarDetailsDto, Car>().
                ReverseMap();
        }
    }
}
