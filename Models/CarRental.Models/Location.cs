using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
