﻿using CarRental.Data.Models.Enums;
using System.Collections.Generic;

namespace CarRental.Data.Models
{
    public class Vehicle
    {

        public Vehicle()
        {
            this.Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Image { get; set; }

        public GearType GearType { get; set; }

        public decimal PricePerDay { get; set; }

        public bool IsRented { get; set; }

        public string LocationId { get; set; }
        public Location Location { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
