using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pizzeria.Core.Models.EntityModels
{
    public class PizzaInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }

        public ICollection<OutletPizza> OutletPizzas { get; set; }
    }
}
