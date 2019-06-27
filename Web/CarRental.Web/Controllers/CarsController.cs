using CarRental.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Web.Controllers
{
    public class CarsController : BaseController
    {
        [HttpPost]
        public IActionResult SearchAll(SearchCarsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", model);
            }

            return Content("Searched");
        }
    }
}
