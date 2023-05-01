using System;
using Test_API.Infrastructure;
using Test_API.Infrastructure.Interfaces;
using Test_API.Models.Orders;
using Test_API.Models.Orders.Api;
using Test_API.Models.Orders.DTO.Api;

namespace Test_API.Services
{
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.

    public class DatabaseService : IOrderBusinessLogic
    {
        private readonly IOrderDataBase _orderRepository;
        private readonly ILogger _logger;
        public DatabaseService(IOrderDataBase orderRepository,ILoggerFactory loggerFactory) 
        {
            _orderRepository = orderRepository;
            _logger = loggerFactory.CreateLogger<DatabaseService>();
        }

        public async Task<FullOrderApiDTO> GetOrderAsync(Guid guid,bool tracking = true)
        {
            try
            {
                var order = await _orderRepository.GetOrderAsync(guid, tracking);
                if (order == null) { return null; }
                return OrderMapper.MapingOrderDtoToFullOrderApiDTO(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<FullOrderApiDTO> CreateOrderAsync(CreateOrderApiDTO createOrder)
        {
            try
            {
                var order = OrderMapper.MapingCreateOrderApiDtoToOrder(createOrder);

                order.Status = OrderStatus.New;
                order.Created = DateTime.Now.ToUniversalTime();
                await _orderRepository.CreateOrderAsync(OrderMapper.MapingOrderToOrderDTO(order));
                return OrderMapper.MapingOrderToFullOrderApiDTO(order);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<FullOrderApiDTO> ChangeOrderAsync(ChangeOrderApiDTO changeOrder, Guid oldOrderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderAsync(oldOrderId);
                order.Products.Clear();
                order.Status = changeOrder.Status;
                order.Products = changeOrder.Products
                    .Select(x=> OrderMapper.MapingProductApiDtoToProductDto(x))
                    .ToList();
                await _orderRepository.ChangeOrderAsync(order);
                return OrderMapper.MapingOrderDtoToFullOrderApiDTO(order);
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
