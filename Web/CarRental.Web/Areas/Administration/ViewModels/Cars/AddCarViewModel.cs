﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Areas.Administration.ViewModels.Cars
{
    public class AddCarViewModel
    {

        private const string PriceRangeError = "The price for the car should be between {1}$ and {2}$";
        private const string MinimumDailyPrice = "5";
        private const string MaximumDailyPrice = "999";
        private const string YearRangeError = "The year for the car should be between {1} and {2}";
        private const string MinimumYear = "1990";
        private const string MaximumYear = "2020";

        [Required]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Desc")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Year")]
        [Range(typeof(int), MinimumYear, MaximumYear, ErrorMessage = YearRangeError)]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [Required]
        [Display(Name = "GearType")]
        public string GearType { get; set; }

        [Required]
        [Display(Name = "PricePerDay")]
        [DataType(DataType.Currency)]
        [Range(typeof(decimal), MinimumDailyPrice, MaximumDailyPrice, ErrorMessage = PriceRangeError)]
        public decimal PricePerDay { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        public ICollection<string> Locations { get; set; }


    }
}
