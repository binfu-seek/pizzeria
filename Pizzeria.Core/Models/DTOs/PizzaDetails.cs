using System;
namespace Pizzeria.Core.Models
{
    public class PizzaDetail
    {
        public int PizzaID { get; set; }
        public string PizzaName { get; set; }
        public string Ingredients { get; set; }
        public int Price { get; set; }
    }
}
