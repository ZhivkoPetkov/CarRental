using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
