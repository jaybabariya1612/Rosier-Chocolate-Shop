using System;
using System.Collections.Generic;

namespace RosierBars.Models
{
    public class OrderModel
    {

        public int OrderId { get; set; }
        public string Mobile { get; set; }
        public string Name{ get; set; }
        public string Email{ get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public int OrderItemId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public List<ProductModel> CartItems{ get; set; }

    }

}