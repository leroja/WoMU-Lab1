using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab1_WOMU.Models.ViewModels
{
    public class OrderViewModel
    {
        public List<Order> allOrders { get; set; }
        public List<OrderRad> allOrderRader { get; set; }
    }
}