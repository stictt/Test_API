using Test_API.Models.Orders.DTO.Db;

namespace Test_API.Infrastructure.Interfaces
{
    public interface IOrderDataBase
    {
        public Task<OrderDTO> GetOrderAsync(Guid id, bool tracking = true);
        public Task CreateOrderAsync(OrderDTO orderDTO);
        public Task DeleteOrderAsync(Guid id);
        public Task ChangeOrderAsync(OrderDTO orderDTO);
    }
}
