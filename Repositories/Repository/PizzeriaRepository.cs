using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Core.Interfaces;
using Pizzeria.Core.Models;
using Pizzeria.Core.Models.EntityModels;

namespace Repositories.Repository
{
    public class PizzeriaRepository : IPizzeriaRepository
    {
        private readonly PizzeriaDbContext _ctx;

        public PizzeriaRepository(PizzeriaDbContext context)
        {
            _ctx = context;
        }

        public async Task<IList<OutletPizzaDetail>> GetAllAsync()
        {
            try
            {
                //var result = await (from outlet in _ctx.Outlets
                //    join shopPizza in _ctx.OutletPizzas on outlet.ID equals shopPizza.OutletID
                //    join pizzaInfo in _ctx.Pizzas on shopPizza.PizzaID equals pizzaInfo.ID
                //    select new OutletPizzaDetail
                //    {
                //        OutletID = outlet.ID,
                //        OutletName = outlet.Name,
                //        PizzaOrderList = outlet.OutletPizzas.Select(sp => new PizzaDetail
                //        {
                //            PizzaID = sp.Pizza.ID,
                //            PizzaName = sp.Pizza.Name,
                //            Ingredients = sp.Pizza.Ingredients,
                //            Price = sp.Price
                //        }).ToList()
                //    }).Distinct().ToListAsync();

                var outletPizzas = await (
                    from outlet in _ctx.Outlets
                    join shopPizza in _ctx.OutletPizzas on outlet.ID equals shopPizza.OutletID
                    join pizzaInfo in _ctx.Pizzas on shopPizza.PizzaID equals pizzaInfo.ID
                    select new { Outlet = outlet, ShopPizza = shopPizza, PizzaInfo = pizzaInfo }
                ).ToListAsync();

                var result = outletPizzas
                    .GroupBy(
                        item => new { item.Outlet.ID, item.Outlet.Name },
                        item => new PizzaDetail
                        {
                            PizzaID = item.ShopPizza.Pizza.ID,
                            PizzaName = item.PizzaInfo.Name,
                            Ingredients = item.PizzaInfo.Ingredients,
                            Price = item.ShopPizza.Price
                        }
                    )
                    .Select(group => new OutletPizzaDetail
                    {
                        OutletID = group.Key.ID,
                        OutletName = group.Key.Name,
                        PizzaOrderList = group.ToList()
                    })
                    .ToList();

                return result;
            } catch (Exception e)
            {
                // Log error
                return null;
            }
        }

        public async Task<OutletPriceChange> AddNewOutletAsync(OutletOpenNew newOutlet)
        {
            // InMemory DB does not support transactions
            //using (var transaction = _ctx.Database.BeginTransaction())
            //{
                try
                {
                    var addedOutlet = _ctx.Outlets.Add(new OutletInfo() { Name = newOutlet.OutletName });
                    await _ctx.SaveChangesAsync();

                    var result = new OutletPriceChange()
                    {
                        OutletID = addedOutlet.Entity.ID,
                        PizzaPriceList = new List<PizzaPrice>()
                    };

                    foreach (var pizza in newOutlet.PizzaPriceList)
                    {
                        var outletPizza = new OutletPizza
                        {
                            OutletID = addedOutlet.Entity.ID,
                            PizzaID = pizza.PizzaID,
                            Price = pizza.Price
                        };

                        _ctx.OutletPizzas.Add(outletPizza);

                        result.PizzaPriceList.Add(new PizzaPrice()
                        {
                            PizzaID = pizza.PizzaID,
                            Price = pizza.Price
                        });
                    }

                    await _ctx.SaveChangesAsync();
                    //transaction.Commit();
                    return result;
                }
                catch
                {
                    //transaction.Rollback();
                    // Log error
                    return null;
                }
            //}
        }

        public async Task<OutletPriceChange> UpdatePizzaPriceAsync(OutletPriceChange changes)
        {
            try
            {
                var existingOutletPizzas = await _ctx.OutletPizzas
                    .Where(sp => sp.OutletID == changes.OutletID)
                    .ToListAsync();

                foreach (var pizzaPrice in changes.PizzaPriceList)
                {
                    var shopPizza = existingOutletPizzas.FirstOrDefault(sp => sp.PizzaID == pizzaPrice.PizzaID);

                    if (shopPizza != null)
                    {
                        shopPizza.Price = pizzaPrice.Price;
                    }
                    else
                    {
                        // add new record when the given pizzaId doesn't exist for the shop
                        existingOutletPizzas.Add(new OutletPizza()
                        {
                            OutletID = changes.OutletID,
                            PizzaID = shopPizza.PizzaID,
                            Price = shopPizza.Price
                        });
                    }
                }

                _ctx.SaveChanges();
                return changes;
            } catch
            {
                // Log error
                return null;
            }
        }
    }
}
