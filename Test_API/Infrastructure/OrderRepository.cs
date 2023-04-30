using Microsoft.EntityFrameworkCore;
using Test_API.Models;
using Test_API.Models.Orders.DTO.Db;

namespace Test_API.Infrastructure
{
    public class OrderRepository
    {
        #pragma warning disable CS8603 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
        private readonly MarketContext _marketContext;
        public OrderRepository(MarketContext context) 
        {
            _marketContext = context;
        }

        public async Task<OrderDTO> GetOrderAsync(Guid id, bool tracking = true)
        {
            if (tracking)
            {
                var order = await _marketContext.Orders
                    .Where(x => x.Id == id)
                    .Include(X => X.Products)
                    .FirstOrDefaultAsync();
                return order;
            }
            else
            {
                var order = await _marketContext.Orders
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Include(X => X.Products)
                    .FirstOrDefaultAsync();
                return order;
            }
        }

        public async Task CreateOrderAsync(OrderDTO orderDTO)
        {
            await _marketContext.Orders.AddAsync(orderDTO);
            await _marketContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var tempOrder = await GetOrderAsync(id);
            _marketContext.Orders.Remove(tempOrder);
            await _marketContext.SaveChangesAsync();
        }

        public async Task ChangeOrderAsync(OrderDTO orderDTO)
        {
            _marketContext.Attach(orderDTO);
            _marketContext.Entry(orderDTO).State = EntityState.Modified;
            await _marketContext.SaveChangesAsync();
        }
    }
}
