using Test_API.Models.Orders.Api;
using Test_API.Models.Orders.DTO.Api;

namespace Test_API.Infrastructure.Interfaces
{
    public interface IOrderBusinessLogic
    {
        public Task<FullOrderApiDTO> GetOrderAsync(Guid guid, bool tracking = true);
        public Task<FullOrderApiDTO> CreateOrderAsync(CreateOrderApiDTO createOrder);
        public Task<FullOrderApiDTO> ChangeOrderAsync(ChangeOrderApiDTO changeOrder, Guid oldOrderId);
        public Task DeleteOrderAsync(Guid id);
    }
}
