using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
