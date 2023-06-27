using System;
namespace Pizzeria.Core.Models.EntityModels
{
    public class OutletPizza
    {
        public int OutletID { get; set; }
        public OutletInfo Outlet { get; set; }

        public int PizzaID { get; set; }
        public PizzaInfo Pizza { get; set; }

        public int Price { get; set; }
    }
}
