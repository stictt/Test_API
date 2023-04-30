using System.ComponentModel.DataAnnotations;

namespace Test_API.Models.Orders.DTO.Db
{
    public class ProductDTO
    {
        [Key]
        public int ProductDTOId { get; set; }
        public Guid Id { get; set; }
        public int Qty { get; set; }
        public Guid OrderDTOId { get; set; }
        public OrderDTO OrderDTO { get; set; }
    }
}
