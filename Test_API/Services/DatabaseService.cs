using System;
using Test_API.Infrastructure;
using Test_API.Models.Orders;

namespace Test_API.Services
{
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.

    public class DatabaseService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ILogger _logger;
        public DatabaseService(OrderRepository orderRepository,ILoggerFactory loggerFactory) 
        {
            _orderRepository = orderRepository;
            _logger = loggerFactory.CreateLogger<DatabaseService>();
        }

        public async Task<Order> GetOrderAsync(Guid guid,bool tracking = true)
        {
            try
            {
                var temp = await _orderRepository.GetOrderAsync(guid, tracking);
                if (temp == null) { return null; }
                return OrderMapper.MapingOrderDtoToOrder(temp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                order.Status = OrderStatus.New;
                order.Created = DateTime.Now.ToUniversalTime();
                await _orderRepository.CreateOrderAsync(OrderMapper.MapingOrderToOrderDTO(order));
                return order;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Order> ChangeOrderAsync(Order order, Order oldOrder)
        {
            try
            {
                var temp = await _orderRepository.GetOrderAsync(oldOrder.Id);
                temp.Products.Clear();
                temp.Id = oldOrder.Id;
                temp.Created = oldOrder.Created;
                temp.Status = order.Status;
                temp.Products = order.Products
                    .Select(x=> OrderMapper.MapingProductToProductDTO(x))
                    .ToList();
                await _orderRepository.ChangeOrderAsync(temp);
                return OrderMapper.MapingOrderDtoToOrder(temp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            try
            {
                await _orderRepository.DeleteOrderAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
