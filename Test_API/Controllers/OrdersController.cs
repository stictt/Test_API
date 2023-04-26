using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Test_API.Infrastructure.Exceptions;
using Test_API.Models.Orders.Api;
using Test_API.Services;

namespace Test_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly DatabaseService _database;
        public OrdersController(DatabaseService database) 
        {
            _database = database;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FullOrderApiDTO>> GetOrder(Guid id)
        {
            try
            {
                return await _database.GetOrderAsync(id);
            }
            catch(NotFoundException e){ return NotFound(); }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                await _database.DeleteOrderAsync(id);
                return Ok();
            }
            catch (NotFoundException e){ return NotFound(); }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FullOrderApiDTO>> PutOrder(Guid id, ChangeOrderApiDTO changeOrder)
        {
            try
            {
                return await _database.ChangeOrderAsync(id, changeOrder);
            }
            catch (NotFoundException e){ return NotFound(); }
            catch (BadRequestException e){ return BadRequest(); }
            catch (ForbiddenException e) { return StatusCode(403); }
            catch
            {
                return StatusCode(500);
            }
        }



        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<FullOrderApiDTO>> PostOrder(CreateOrderApiDTO order)
        {
            try
            {
                return await _database.CreateOrderAsync(order);
            }
            catch (BadRequestException e) { return BadRequest(); }
            catch (ConflictException e){ return Conflict(); }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
