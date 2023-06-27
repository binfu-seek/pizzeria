using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pizzeria.Core.Models;

namespace Pizzeria.Core.Interfaces
{
    public interface IPizzeriaService
    {
        Task<IList<OutletPizzaDetail>> GetAllAsync();
        Task<int> GetOrderPrice(CustomerOrder order);
        Task<OutletPriceChange> UpdatePizzaPriceAsync(OutletPriceChange changes);
        Task<OutletPriceChange> OpenNewOutletAsync(OutletOpenNew newOutlet);
    }
}
