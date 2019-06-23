namespace CarRental.Data.Models
{
    public class Review
    {
        public string Id { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
