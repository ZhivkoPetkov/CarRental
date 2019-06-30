using System;

namespace CarRental.Web.InputModels.Orders
{
    public class OrderPreviewInputModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public decimal PricePerDay { get; set; }

        public DateTime RentStart { get; set; }

        public DateTime RentEnd { get; set; }

        public int Days { get; set; }

        public string Image { get; set; }

        public string PickUpPlace { get; set; }

        public string ReturnPlace { get; set; }
    }
}
