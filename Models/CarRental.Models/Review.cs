using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }

        public virtual Car Car { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser User { get; set; }
        [Required]
        [Range(1,5)]
        public int Rating { get; set; }
        [Required]
        [MinLength(16)]
        public string Comment { get; set; }
    }
}
