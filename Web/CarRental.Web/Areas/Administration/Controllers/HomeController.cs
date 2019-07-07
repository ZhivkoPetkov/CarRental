using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class HomeController : AdministrationController
    {
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}