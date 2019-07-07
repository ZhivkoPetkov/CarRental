using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.InputModels.Reviews
{
    public class ReviewInputModel
    {

        private const int minimumLenghtComment = 16;
        private const int maximumLenghtComment = 450;
        private const string minimumLenghtError = "Your comment must be minimum {1} symbols.";
        private const string maximumLenghtError = "Your comment must be maximum {1} symbols.";

        [Required]
        [MinLength(minimumLenghtComment, ErrorMessage = minimumLenghtError)]
        [MaxLength(maximumLenghtComment, ErrorMessage = maximumLenghtError)]
        public string Comment { get; set; }
        [Required]
        public int Rating { get; set; }

        [Required]
        public string OrderId { get; set; }
    }
}
