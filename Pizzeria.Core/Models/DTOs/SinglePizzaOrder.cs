using System;
namespace Pizzeria.Core.Models
{
    public class SinglePizzaOrder
    {
        public int PizzaID { get; set; }
        public int Quantity { get; set; }
        public int? ToppingID { get; set; }
    }
}
