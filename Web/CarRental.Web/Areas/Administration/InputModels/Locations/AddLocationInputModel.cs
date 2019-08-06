using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Areas.Administration.InputModels.Locations
{
    public class AddLocationInputModel
    {

        private const string ErrorLength = "The name of the location should be between {2} and {1} symbols";

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = ErrorLength)]
        public string Name { get; set; }

        public ICollection<string> Locations { get; set; }
    }
}
