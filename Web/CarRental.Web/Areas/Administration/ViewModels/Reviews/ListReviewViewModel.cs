namespace CarRental.Web.Areas.Administration.ViewModels.Reviews
{
    public class ListReviewViewModel
    {
        public int Id { get; set; }

        public string CarModel { get; set; }

        public string User { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
