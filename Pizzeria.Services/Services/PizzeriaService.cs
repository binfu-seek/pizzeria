using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pizzeria.Core.Interfaces;
using Pizzeria.Core.Models;

namespace Pizzeria.Services.Services
{
    public class PizzeriaService : IPizzeriaService
    {
        private readonly IPizzeriaRepository _repository;

        private const int TOPPING_PRICE = 1;

        public PizzeriaService(IPizzeriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<OutletPizzaDetail>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<int> GetOrderPrice(CustomerOrder order)
        {
            var allOutlets = await _repository.GetAllAsync();
            var pizzaPriceList = allOutlets.Where(o => o.OutletID == order.OutletID)
                .Select(o => o.PizzaOrderList).FirstOrDefault();

            if (pizzaPriceList != null)
            {
                int totalPrice = 0;
                foreach(var pizza in order.PizzaOrderList)
                {
                    var pizzaDetail = pizzaPriceList.FirstOrDefault(p => p.PizzaID == pizza.PizzaID);

                    if (pizzaDetail != null)
                    {
                        totalPrice += pizzaDetail.Price * pizza.Quantity;

                        // TODO: need to check if the value of toppingID exists in database
                        if (pizza.Quantity > 0)
                        {
                            totalPrice += pizza.ToppingID.HasValue ? TOPPING_PRICE : 0;
                        }
                    } else
                    {
                        throw new Exception("Ordered pizza does not sell in this outlet!");
                    }
                }
                return totalPrice;
            } else
            {
                throw new Exception("Outlet does not exist!");
            }
        }

        public async Task<OutletPriceChange> OpenNewOutletAsync(OutletOpenNew newOutlet)
        {
            return await _repository.AddNewOutletAsync(newOutlet);
        }

        public async Task<OutletPriceChange> UpdatePizzaPriceAsync(OutletPriceChange changes)
        {
            return await _repository.UpdatePizzaPriceAsync(changes);
        }
    }
}
