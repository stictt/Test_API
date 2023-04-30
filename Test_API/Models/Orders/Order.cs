namespace Test_API.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Created { get; set; }
        public List<Product> Products { get; set; }
    }
}
