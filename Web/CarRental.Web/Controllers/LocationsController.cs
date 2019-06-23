using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class LocationsController : BaseController
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}