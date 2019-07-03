using CarRental.DTOs.Reviews;
using CarRental.Models;
using CarRental.Models.Enums;
using CarRental.Web.ViewModels.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewModels.Cars
{
    public class CarDetailsViewModel
    {
        public CarDetailsViewModel()
        {
            this.Reviews = new HashSet<ReviewViewModel>();
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

        public ICollection<ReviewViewModel> Reviews { get; set; }
    }
}
