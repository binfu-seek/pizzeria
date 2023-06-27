using System;
using System.Collections.Generic;

namespace Pizzeria.Core.Models
{
    public class CustomerOrder
    {
        public int OutletID { get; set; }
        public List<SinglePizzaOrder> PizzaOrderList { get; set; }
    }
}
