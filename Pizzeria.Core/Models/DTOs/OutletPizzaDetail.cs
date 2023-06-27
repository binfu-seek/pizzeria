using System;
using System.Collections.Generic;

namespace Pizzeria.Core.Models
{
    public class OutletPizzaDetail
    {
        public int OutletID { get; set; }
        public string OutletName { get; set; }
        public List<PizzaDetail> PizzaOrderList { get; set; }
    }
}
