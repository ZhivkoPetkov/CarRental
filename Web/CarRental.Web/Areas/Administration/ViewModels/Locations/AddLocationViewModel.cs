using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Areas.Administration.ViewModels.Locations
{
    public class AddLocationViewModel
    {
        [Required]
        [Display(Name = "Name")]
        [MinLength(8)]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<string> Locations { get; set; }
    }
}
