using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Models
{
   public class CarRentDays
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        public DateTime RentDate { get; set; }
    }
}
