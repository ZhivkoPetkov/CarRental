using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace CarRental.Web.ModelBinders
{
    public class LocationModelBinder : IModelBinder
    {
        private readonly ILocationsService locationsService;

        public LocationModelBinder(ILocationsService locationsService)
        {
            this.locationsService = locationsService;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var locationName = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            var locationId = this.locationsService.GetIdByName(locationName);

            if (locationId != null)
            {
                bindingContext.Result = ModelBindingResult.Success(locationId);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
