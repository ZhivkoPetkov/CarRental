using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.ViewModels.Home
{
    public class SearchCarsViewModel : IValidatableObject
    {
        private const string PickupError = "Pick Up date cannot be after the return date!";
        private const string PastDate = "Pick Up date cannot be in the past!";

        public SearchCarsViewModel()
        {
            this.Pickup = DateTime.UtcNow;
            this.Return = DateTime.UtcNow;
            this.Return.AddDays(1);
        }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Pick Up Date")]
        public DateTime Pickup { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime Return { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Pick Up Place")]
        public string PickupPlace { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Return Place")]
        public string ReturnPlace{ get; set; }

        public ICollection<string> Locations { get; set; } = new HashSet<string>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Pickup.Date >= Return.Date)
            {
                yield return new ValidationResult(PickupError);
            }

            if ((Pickup.Date.Month <= DateTime.UtcNow.Month) && Pickup.Date.Day < DateTime.UtcNow.Day)
            {
                yield return new ValidationResult(PastDate);
            }
        }
    }
}
