using AutoMapper;
using CarRental.DTOs.Cars;
using CarRental.DTOs.Locations;
using CarRental.DTOs.Orders;
using CarRental.DTOs.Reviews;
using CarRental.DTOs.Users;
using CarRental.DTOs.Vouchers;
using CarRental.Models;
using CarRental.Web.Areas.Administration.InputModels.Cars;
using CarRental.Web.Areas.Administration.ViewModels.Reviews;
using CarRental.Web.Areas.Administration.ViewModels.Vouchers;
using CarRental.Web.ViewModels.Cars;
using CarRental.Web.ViewModels.Orders;
using CarRental.Web.ViewModels.Reviews;
using CarRental.Web.ViewModels.Vouchers;
using System.Linq;
using CarRental.Web.Areas.Administration.InputModels.Locations;

namespace CarRental.Web.MappingConfiguration
{
    public class CarRentalConfiguration : Profile
    {
        public CarRentalConfiguration()
        {
            this.CreateMap<AddLocationInputModel, Location>();
            this.CreateMap<Order, OrderDto>();
            this.CreateMap<OrderDto, MyOrdersViewModel>()
                   .ForMember(dest => dest.PickUpLocation, src => src.MapFrom(x => x.PickUpLocation.Name))
                   .ForMember(dest => dest.ReturnLocation, src => src.MapFrom(x => x.ReturnLocation.Name))
                   .ForMember(dest => dest.CarModel, src => src.MapFrom(x => x.Car.Model))
                   .ReverseMap();
            this.CreateMap<OrderDto, OrderDetailsViewModel>()
                  .ForMember(dest => dest.CarModel, src => src.MapFrom(x => x.Car.Model))
                  .ForMember(dest => dest.CarGearType, src => src.MapFrom(x => x.Car.GearType))
                  .ForMember(dest => dest.CarYear, src => src.MapFrom(x => x.Car.Year))
                  .ForMember(dest => dest.Email, src => src.MapFrom(x => x.User.Email))
                  .ForMember(dest => dest.Firstname, src => src.MapFrom(x => x.User.FirstName))
                  .ForMember(dest => dest.Lastname, src => src.MapFrom(x => x.User.LastName))
                  .ForMember(dest => dest.PickUpLocation, src => src.MapFrom(x => x.PickUpLocation.Name))
                  .ForMember(dest => dest.ReturnLocation, src => src.MapFrom(x => x.ReturnLocation.Name))
                  .ForMember(dest => dest.CarImage, src => src.MapFrom(x => x.Car.Image))
                  .ForMember(dest => dest.CarDescription, src => src.MapFrom(x => x.Car.Description))
                  .ForMember(dest => dest.Rating, src => src.MapFrom(x => x.Review.Rating))
                  .ForMember(dest => dest.Comment, src => src.MapFrom(x => x.Review.Comment))
                  .ReverseMap();
            this.CreateMap<OrderDto, OrderEditViewModel>()
                  .ForMember(dest => dest.Email, src => src.MapFrom(x => x.User.Email))
                  .ForMember(dest => dest.Firstname, src => src.MapFrom(x => x.User.FirstName))
                  .ForMember(dest => dest.Lastname, src => src.MapFrom(x => x.User.LastName))
                  .ForMember(dest => dest.ReturnLocation, src => src.MapFrom(x => x.ReturnLocation.Name))
                  .ForMember(dest => dest.PickUpLocation, src => src.MapFrom(x => x.PickUpLocation.Name));
            this.CreateMap<AddCarInputModel, Car>();
            this.CreateMap<CarEditViewModel, CarDetailsDto>().ReverseMap();
            this.CreateMap<ListCarDto, Car>();
            this.CreateMap<ApplicationUser, UserDto>();
            this.CreateMap<ReviewDto, Review>().ReverseMap();
            this.CreateMap<ReviewViewModel, ReviewDto>().ReverseMap();
            this.CreateMap<CarDetailsDto, CarDetailsViewModel>().ReverseMap();
            this.CreateMap<CarEditViewModel, Car>();
            this.CreateMap<CarDetailsDto, Car>().ReverseMap();
            this.CreateMap<Voucher, VoucherDto>();
            this.CreateMap<Location, LocationDto>();
            this.CreateMap<VoucherViewModel, VoucherDto>().ReverseMap();
            this.CreateMap<Review, ListReviewDto>()
                .ForMember(dest => dest.CarModel, src => src.MapFrom(x => x.Car.Model))
                .ForMember(dest => dest.User, src => src.MapFrom(x => x.User.Email));
            this.CreateMap<ListReviewDto, ListReviewViewModel>();
            this.CreateMap<UserVouchersDto, UserVoucherViewModel>();
            this.CreateMap<ApplicationUser, UserVouchersDto>()
                 .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                 .ForMember(dest => dest.Rents, src => src.MapFrom(x => x.Orders.Count))
                 .ForMember(dest => dest.MoneySpent, src => src.MapFrom(x => x.Orders.Sum(p => p.Price)));
            this.CreateMap<VoucherDto, VoucherDetailsViewModel>()
                 .ForMember(dest => dest.User, src => src.MapFrom(x => x.User.Email));

        }
    }
}
