using Test_API.Models.Orders;

namespace Test_API.Services
{
    public class OrderValidationService
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
