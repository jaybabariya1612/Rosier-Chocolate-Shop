using System;

namespace RosierBars.Models
{
    public class OrderDetailModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string Mobile { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string imageurl { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}