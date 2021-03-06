﻿using CarRental.DTOs.Reviews;
using CarRental.Models;
using CarRental.Models.Enums;
using System.Collections.Generic;

namespace CarRental.DTOs.Cars
{
   public class CarDetailsDto
    {
        public CarDetailsDto()
        {
            this.Reviews = new HashSet<ReviewDto>(); 
        }
        public int Id { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public string Image { get; set; }

        public GearType GearType { get; set; }

        public decimal PricePerDay { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public ICollection<ReviewDto> Reviews { get; set; }

    }
}
