﻿using Test_API.Infrastructure;

namespace Test_API.Models.Orders.DbDTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Created { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
