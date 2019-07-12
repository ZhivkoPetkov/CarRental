
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarRental.Models.Enums;

namespace CarRental.Models
{
    public class Car
    {
        public Car()
        {
            this.Reviews = new HashSet<Review>();
            this.RentDays = new HashSet<CarRentDays>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Model { get; set; }

        [Required]
        [MinLength(150)]
        public string Description { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public GearType GearType { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        [Required]
        public int LocationId { get; set; }
         public virtual Location Location { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<CarRentDays> RentDays { get; set; }
    }
}
