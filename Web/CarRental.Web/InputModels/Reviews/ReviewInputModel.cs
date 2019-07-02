using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.InputModels.Reviews
{
    public class ReviewInputModel
    {
        [Required]
        [MinLength(50)]
        [MaxLength(450)]
        public string Comment { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}
