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