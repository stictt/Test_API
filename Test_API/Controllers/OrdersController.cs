using Microsoft.AspNetCore.Mvc;
using Test_API.Infrastructure;
using Test_API.Infrastructure.Interfaces;
using Test_API.Models.Orders.Api;
using Test_API.Models.Orders.DTO.Api;
using Test_API.Services;

namespace Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderBusinessLogic _databaseService;
        private readonly IValidationOrder _orderValidationService;
        public OrdersController(IOrderBusinessLogic database, IValidationOrder validationService) 
        {
            _databaseService = database;
            _orderValidationService = validationService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FullOrderApiDTO>> GetOrder(Guid id)
        {
            var order = await _databaseService.GetOrderAsync(id, false);

            if (order == null) { return NotFound(); }

            return order;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _databaseService.GetOrderAsync(id, false);
            if (order == null) { return NotFound(); }
            if (!_orderValidationService.IsAvailabilityChanges(order)) { return StatusCode(403); }
            await _databaseService.DeleteOrderAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FullOrderApiDTO>> PutOrder(Guid id, ChangeOrderApiDTO changeOrder)
        {
            var order = await _databaseService.GetOrderAsync(id,false);
            if (order == null) { return NotFound(); }

            if (!_orderValidationService.IsAvailabilityChanges(order)) { return StatusCode(403); }
            if (!_orderValidationService.IsValidChangeOrderApiDTO(changeOrder)) { return BadRequest(); }

            var orderView = await _databaseService.ChangeOrderAsync(changeOrder, id);
            return orderView;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<FullOrderApiDTO>> PostOrder(CreateOrderApiDTO orderCreate)
        {
            var order = await _databaseService.GetOrderAsync(orderCreate.Id, false);
            if (order != null) { return Conflict(); }

            if (!_orderValidationService.IsValidCreateOrderApiDTO(orderCreate)) { return BadRequest(); }

            var orderView = await _databaseService.CreateOrderAsync(orderCreate);
            return orderView;
        }
    }
}
