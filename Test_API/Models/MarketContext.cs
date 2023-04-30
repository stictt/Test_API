using Microsoft.EntityFrameworkCore;
using Test_API.Models.Orders;
using Test_API.Models.Orders.DTO.Db;

namespace Test_API.Models
{
    public class MarketContext : DbContext
    {
        public MarketContext(DbContextOptions<MarketContext> options) : base(options)
        {
            Database.Migrate();
            this.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OrderDTO>()
                .Property(x => x.Status)
                 .HasConversion(
                    x => x.ToString(),
                    x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));

            modelBuilder.Entity<OrderDTO>()
            .HasMany(x => x.Products)
            .WithOne(x => x.OrderDTO)
            .HasForeignKey(b => b.OrderDTOId)
            .OnDelete(DeleteBehavior.ClientCascade);
        }
        public DbSet<OrderDTO> Orders { get; set; }
    }
}
