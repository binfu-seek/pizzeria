using System;
using System.Collections.Generic;

namespace Pizzeria.Core.Models
{
    public class OutletOpenNew
    {
        public string OutletName { get; set; }
        public List<PizzaPrice> PizzaPriceList { get; set; }
    }
}
