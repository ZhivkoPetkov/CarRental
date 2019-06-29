using AutoMapper;
using CarRental.DTOs.Cars;
using CarRental.Models;
using CarRental.Web.Areas.Administration.ViewModels.Cars;
using CarRental.Web.Areas.Administration.ViewModels.Locations;

namespace CarRental.Web.MappingConfiguration
{
    public class CarRentalConfiguration : Profile
    {
        public CarRentalConfiguration()
        {
            this.CreateMap<AddLocationViewModel, Location>();
            this.CreateMap<AddCarViewModel, Car>();
            this.CreateMap<ListCarDto, Car>();
            this.CreateMap<CarDetailsDto, Car>().
                ReverseMap();
        }
    }
}
