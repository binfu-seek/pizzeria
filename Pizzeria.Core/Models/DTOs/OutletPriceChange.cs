using System;
using System.Collections.Generic;

namespace Pizzeria.Core.Models
{
    public class OutletPriceChange
    {
        public int OutletID { get; set; }
        public List<PizzaPrice> PizzaPriceList { get; set; }
    }
}
