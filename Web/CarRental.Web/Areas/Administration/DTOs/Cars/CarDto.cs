using CarRental.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace CarRental.Web.Areas.Administration.DTOs.Cars
{
    public class CarDto
    {
        public string Model { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public IFormFile Image { get; set; }

        public GearType GearType { get; set; }

        public decimal PricePerDay { get; set; }

        public string LocationId { get; set; }

    }
}
