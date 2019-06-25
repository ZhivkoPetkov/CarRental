using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class CarsController : AdministrationController
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}