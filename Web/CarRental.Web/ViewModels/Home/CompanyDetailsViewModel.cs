using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewModels.Home
{
    public class CompanyDetailsViewModel
    {
        public int Clients { get; set; }
        public int Cars { get; set; }
        public int Reviews { get; set; }
        public double Rating { get; set; }
    }
}
