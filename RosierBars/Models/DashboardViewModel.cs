using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosierBars.Models
{
    public class DashboardViewModel
    {
        public List<OrderModel> RecentOrders { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

}