using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CarRental.Common;
using CarRental.Data;
using CarRental.DTOs.Cars;
using CarRental.DTOs.Locations;
using CarRental.DTOs.Orders;
using CarRental.DTOs.Reviews;
using CarRental.DTOs.Users;
using CarRental.DTOs.Vouchers;
using CarRental.Models;
using CarRental.Web.Areas.Administration.InputModels.Cars;
using CarRental.Web.Areas.Administration.ViewModels.Locations;
using CarRental.Web.Areas.Administration.ViewModels.Rewiews;
using CarRental.Web.Areas.Administration.ViewModels.Vouchers;
using CarRental.Web.ViewModels.Cars;
using CarRental.Web.ViewModels.Orders;
using CarRental.Web.ViewModels.Reviews;
using CarRental.Web.ViewModels.Vouchers;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services.Tests
{
    public abstract class BaseServiceTests
    {
        protected readonly IMapper mapper;
        protected readonly Cloudinary cloudinary;

        private const string cloudName = "dis59vn8s";
        private const string cloudApi = "843947874516971";
        private const string cloudKey = "Kn7P9mTbpt2pflIJCxUs7lFsC_Y";

        public BaseServiceTests()
        {
            this.mapper = InitializeMapper();
            this.cloudinary = InitializeCloudinary();
        }

        private Cloudinary InitializeCloudinary()
        {
         
            var cloudinaryAccount = new CloudinaryDotNet.Account(cloudName, cloudApi, cloudKey);
            return new Cloudinary(cloudinaryAccount);
        }

        private IMapper InitializeMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<AddLocationViewModel, Location>();
                cfg.CreateMap<Order, OrderDto>();
                cfg.CreateMap<OrderDto, MyOrdersViewModel>()
                       .ForMember(dest => dest.PickUpLocation, src => src.MapFrom(x => x.PickUpLocation.Name))
                       .ForMember(dest => dest.ReturnLocation, src => src.MapFrom(x => x.ReturnLocation.Name))
                       .ForMember(dest => dest.CarModel, src => src.MapFrom(x => x.Car.Model))
                       .ReverseMap();
                cfg.CreateMap<OrderDto, OrderDetailsViewModel>()
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
                cfg.CreateMap<OrderDto, OrderEditViewModel>()
                      .ForMember(dest => dest.Email, src => src.MapFrom(x => x.User.Email))
                      .ForMember(dest => dest.Firstname, src => src.MapFrom(x => x.User.FirstName))
                      .ForMember(dest => dest.Lastname, src => src.MapFrom(x => x.User.LastName));
                cfg.CreateMap<AddCarViewModel, Car>();
                cfg.CreateMap<CarEditViewModel, CarDetailsDto>().ReverseMap();
                cfg.CreateMap<ListCarDto, Car>();
                cfg.CreateMap<ApplicationUser, UserDto>();
                cfg.CreateMap<ReviewDto, Review>().ReverseMap();
                cfg.CreateMap<ReviewViewModel, ReviewDto>().ReverseMap();
                cfg.CreateMap<CarDetailsDto, CarDetailsViewModel>().ReverseMap();
                cfg.CreateMap<CarEditViewModel, Car>();
                cfg.CreateMap<CarDetailsDto, Car>().ReverseMap();
                cfg.CreateMap<Voucher, VoucherDto>();
                cfg.CreateMap<Location, LocationDto>();
                cfg.CreateMap<VoucherViewModel, VoucherDto>().ReverseMap();
                cfg.CreateMap<Review, ListReviewDto>()
                    .ForMember(dest => dest.CarModel, src => src.MapFrom(x => x.Car.Model))
                    .ForMember(dest => dest.User, src => src.MapFrom(x => x.User.Email));
                cfg.CreateMap<ListReviewDto, ListReviewViewModel>();
                cfg.CreateMap<UserVouchersDto, UserVoucherViewModel>();
                cfg.CreateMap<ApplicationUser, UserVouchersDto>()
                     .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email))
                     .ForMember(dest => dest.Rents, src => src.MapFrom(x => x.Orders.Count))
                     .ForMember(dest => dest.MoneySpent, src => src.MapFrom(x => x.Orders.Sum(p => p.Price)));
                cfg.CreateMap<VoucherDto, VoucherDetailsViewModel>()
                     .ForMember(dest => dest.User, src => src.MapFrom(x => x.User.Email));

            });

            return configuration.CreateMapper();
        }
    }
}
