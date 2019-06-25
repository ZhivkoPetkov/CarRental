using AutoMapper;
using CarRental.Models;
using CarRental.Web.Areas.Administration.ViewModels.Locations;

namespace CarRental.Web.MappingConfiguration
{
    public class CarRentalConfiguration : Profile
    {
        public CarRentalConfiguration()
        {
            this.CreateMap<AddLocationViewModel, Location>();
        }
    }
}
