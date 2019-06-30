using CarRental.Models;
using CarRental.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.DTOs.Cars
{
    public class ListCarDto
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Image { get; set; }

        public GearType GearType { get; set; }

        public decimal PricePerDay { get; set; }

        public string Location { get; set; }

        public DateTime StartRent {get;set;}
        public DateTime End { get; set; }
        public int Days { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } 

        public virtual ICollection<CarRentDays> RentDays { get; set; }

    }
}
