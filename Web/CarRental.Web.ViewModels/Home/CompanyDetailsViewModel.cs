using System;
using System.Collections.Generic;
using System.Text;

namespace CarRental.Web.ViewModels.Home
{
    public class CompanyDetailsViewModel
    {
        public int Clients { get; set; }
        public int Cars { get; set; }
        public int Reviews { get; set; }
        public string Rating { get; set; }
    }
}
