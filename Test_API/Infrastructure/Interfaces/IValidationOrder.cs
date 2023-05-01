using Test_API.Models.Orders;
using Test_API.Models.Orders.Api;
using Test_API.Models.Orders.DTO.Api;

namespace Test_API.Infrastructure.Interfaces
{
    public interface IValidationOrder
    {
        public bool IsValidChangeOrderApiDTO(ChangeOrderApiDTO changeOrder);
        public bool IsValidCreateOrderApiDTO(CreateOrderApiDTO createOrder);
        public bool IsValidOrder(Order order);
        public bool IsAvailabilityChanges(Order order);
        public bool IsAvailabilityChanges(FullOrderApiDTO order);
        public bool IsAvailabilityChanges(ChangeOrderApiDTO order);
    }
}
