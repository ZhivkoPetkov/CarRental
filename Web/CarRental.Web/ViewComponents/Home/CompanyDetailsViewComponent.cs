using CarRental.Data;
using CarRental.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Web.ViewComponents.Home
{
    public class CompanyDetailsViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly CarRentalDbContext dbContext;

        public CompanyDetailsViewComponent(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetStats();
            return View(items);
        }

        private Task<CompanyDetailsViewModel> GetStats()
        {
            var model = new CompanyDetailsViewModel
            {
                Cars = this.dbContext.Cars.Count(),
                Clients = this.dbContext.Users.Count(),
                Rating = this.dbContext.Reviews.Count() == 0 ? "0" : this.dbContext.Reviews.Average(p => p.Rating).ToString("F"),
                Reviews = this.dbContext.Reviews.Count()
            };

            return Task.FromResult(model);
        }
    }
}
