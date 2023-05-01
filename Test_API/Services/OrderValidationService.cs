using Test_API.Infrastructure;
using Test_API.Infrastructure.Interfaces;
using Test_API.Models.Orders;
using Test_API.Models.Orders.Api;
using Test_API.Models.Orders.DTO.Api;

namespace Test_API.Services
{
    public class OrderValidationService : IValidationOrder
    {
        public bool IsAvailabilityChanges(Order order)
        {
            switch (order.Status)
            {
                case OrderStatus.Completed:
                case OrderStatus.Delivered:
                case OrderStatus.HandedForDelivery:
                    return false;
            }
            return true;
        }

        public bool IsAvailabilityChanges(ChangeOrderApiDTO order)
        {
            var changeOrder = OrderMapper.MapingChangeOrderApiDtoToOrder(order);
            return IsAvailabilityChanges(changeOrder);
        }

        public bool IsAvailabilityChanges(FullOrderApiDTO order)
        {
            var changeOrder = OrderMapper.MapingFullOrderApiDtoToOrder(order);
            return IsAvailabilityChanges(changeOrder);
        }

        public bool IsValidChangeOrderApiDTO(ChangeOrderApiDTO changeOrder)
        {
            var order = OrderMapper.MapingChangeOrderApiDtoToOrder(changeOrder);
            return IsValidOrder(order);
        }

        public bool IsValidCreateOrderApiDTO(CreateOrderApiDTO createOrder)
        {
            var order = OrderMapper.MapingCreateOrderApiDtoToOrder(createOrder);
            return IsValidOrder(order);
        }

        public bool IsValidOrder(Order order)
        {
            bool isValid = true;
            if (order.Products.Count == 0)
            {
                return false;
            }

            order.Products.ForEach(x =>
            {
                if (x.Qty <= 0) 
                {  
                    isValid = false; 
                }
            });
            return isValid;
        }
    }
}
