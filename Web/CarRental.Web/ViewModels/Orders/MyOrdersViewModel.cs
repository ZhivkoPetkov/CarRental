using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewModels.Orders
{
    public class MyOrdersViewModel
    {
        public string Id { get; set; }

        public string CarModel { get; set; }
        public string PickUpLocation { get; set; }
        public string ReturnLocation { get; set; }
        public decimal Price { get; set; }
    }
}
