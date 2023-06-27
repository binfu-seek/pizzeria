using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pizzeria.Core.Interfaces;
using Pizzeria.Core.Models;

namespace Pizzeria.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PizzeriaController : ControllerBase
    {

        private readonly ILogger<PizzeriaController> _logger;
        private readonly IPizzeriaService _service;

        public PizzeriaController(ILogger<PizzeriaController> logger, IPizzeriaService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IList<OutletPizzaDetail>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("totalPrice")]
        public async Task<ActionResult<int>> GetTotalPriceAsync([FromBody] CustomerOrder order)
        {
            try
            {
                var result = await _service.GetOrderPrice(order);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        //[Authorize]   // TODO: implement authentication functionalities
        [Route("newOutlet")]
        public async Task<ActionResult<OutletPriceChange>> OpenNewOutletAsync([FromBody]  OutletOpenNew newOutlet)
        {
            var result = await _service.OpenNewOutletAsync(newOutlet);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize]   // TODO: implement authentication functionalities
        [Route("updatePrice")]
        public async Task<ActionResult<OutletPriceChange>> UpdatePizzaPriceAsync([FromBody] OutletPriceChange changes)
        {
            var result = await _service.UpdatePizzaPriceAsync(changes);
            return Ok(result);
        }
    }
}
