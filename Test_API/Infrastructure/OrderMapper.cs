using AutoMapper;
using Test_API.Models.Orders;
using Test_API.Models.Orders.Api;
using Test_API.Models.Orders.DTO.Api;
using Test_API.Models.Orders.DTO.Db;

namespace Test_API.Infrastructure
{
    public static class OrderMapper
    {
        public static Order MapingOrderDtoToOrder(OrderDTO orderDTO)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, Order>()
                        .ForMember(x => x.Products,
                            y => y.MapFrom(s => s.Products));
                cfg.CreateMap<ProductDTO, Product>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<Order>(orderDTO);
        }

        public static OrderDTO MapingOrderToOrderDTO(Order order)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDTO>()
                        .ForMember(x => x.Products,
                            y => y.MapFrom(s => s.Products));
                cfg.CreateMap<Product, ProductDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<OrderDTO>(order);
        }

        public static FullOrderApiDTO MapingOrderToFullOrderApiDTO(Order order)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, FullOrderApiDTO>()
                        .ForMember(x => x.Products,
                            y => y.MapFrom(s => s.Products));
                cfg.CreateMap<Product, ProductApiDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<FullOrderApiDTO>(order);
        }

        public static Order MapingCreateOrderApiDtoToOrder(CreateOrderApiDTO createOrder)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrderApiDTO, Order>()
                        .ForMember(x => x.Products,
                            y => y.MapFrom(s => s.Products));
                cfg.CreateMap<ProductApiDTO, Product>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<Order>(createOrder);
        }

        public static Order MapingChangeOrderApiDtoToOrder(ChangeOrderApiDTO createOrder)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChangeOrderApiDTO, Order>()
                        .ForMember(x => x.Products,
                            y => y.MapFrom(s => s.Products));
                cfg.CreateMap<ProductApiDTO, Product>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<Order>(createOrder);
        }

        public static ProductDTO MapingProductToProductDTO(Product product)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<ProductDTO>(product);
        }
    }
}
