using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pizzeria.Core.Models;

namespace Pizzeria.Core.Interfaces
{
    public interface IPizzeriaRepository
    {
        Task<IList<OutletPizzaDetail>> GetAllAsync();
        Task<OutletPriceChange> UpdatePizzaPriceAsync(OutletPriceChange changes);
        Task<OutletPriceChange> AddNewOutletAsync(OutletOpenNew newOutlet);
    }
}
