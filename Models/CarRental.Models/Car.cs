using CarRental.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Car
    {

        public Car()
        {
            this.Reviews = new HashSet<Review>();
            this.RentDays = new HashSet<CarRentDays>();
            this.IsRented = false;
        }

        [Key]
        public int Id { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Image { get; set; }

        public GearType GearType { get; set; }

        public decimal PricePerDay { get; set; }

        public bool IsRented { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<CarRentDays> RentDays { get; set; }
    }
}
