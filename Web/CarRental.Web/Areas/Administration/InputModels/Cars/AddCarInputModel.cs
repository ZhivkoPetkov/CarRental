using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarRental.Web.ModelBinders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Areas.Administration.InputModels.Cars
{
    public class AddCarInputModel
    {

        private const string PriceRangeError = "The price for the car should be between {1}$ and {2}$";
        private const string MinimumDailyPrice = "5";
        private const string MaximumDailyPrice = "999";

        private const string YearRangeError = "The year for the car should be between {1} and {2}";
        private const string MinimumYear = "1990";
        private const string MaximumYear = "2020";

        private const int MaximumModelLenght = 5;
        private const string MinimumLenghtModelError = "The Model name should be atleast {1} symbols";

        private const int MaximumDescriptionlLenght = 150;
        private const string MaximumDescriptionlLenghtError = "The Description should be atleast {1} symbols";

        public AddCarInputModel()
        {
            this.Year = DateTime.UtcNow.Year;
        }

        [Required]
        [Display(Name = "Model")]
        [MinLength(MaximumModelLenght, ErrorMessage = MinimumLenghtModelError)]
        public string Model { get; set; }

        [Required]
        [Display(Name = "Desc")]
        [DataType(DataType.MultilineText)]
        [MinLength(MaximumDescriptionlLenght, ErrorMessage = MaximumDescriptionlLenghtError)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Year")]
        [Range(typeof(int), MinimumYear, MaximumYear, ErrorMessage = YearRangeError)]
        public int Year { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

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
        [ModelBinder(typeof(LocationModelBinder))]
        public int LocationId { get; set; }

        public ICollection<string> Locations { get; set; }
    }
}
