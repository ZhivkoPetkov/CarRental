using CarRental.DTOs.Cars;
using System;
using System.Collections.Generic;

namespace CarRental.Web.ViewModels.Cars
{
    public class AvailableCarsViewModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Days { get; set; }

        public ICollection<ListCarDto> Cars { get; set; }

        public string PickUpPlace { get; set; }
        public string ReturnPlace { get; set; }
    }
}
