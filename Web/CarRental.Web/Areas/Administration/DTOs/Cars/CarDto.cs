using CarRental.Models.Enums;

namespace CarRental.Web.Areas.Administration.DTOs.Cars
{
    public class CarDto
    {
        public string Model { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Image { get; set; }

        public GearType GearType { get; set; }

        public decimal PricePerDay { get; set; }

        public string LocationId { get; set; }

    }
}
