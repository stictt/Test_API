using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using Test_API.Infrastructure.Exceptions;
using Test_API.Models;
using Test_API.Models.Orders.Api;
using Test_API.Models.Orders.DbDTO;

namespace Test_API.Services
{
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.

    public class DatabaseService
    {
        private readonly MarketContext _context;
        private readonly ILogger _logger;
        public DatabaseService(MarketContext context,ILoggerFactory loggerFactory) 
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<DatabaseService>();
        }

        public async Task<FullOrderApiDTO> GetOrderAsync(Guid guid)
        {
            OrderDTO orderDTO = null;

            try
            {
                orderDTO = await _context.Orders
                    .Where(x=>x.Id == guid)
                    .Include(X=>X.Products)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            if (orderDTO != null)
            {
                return MapOrderDtoToFull(orderDTO);
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async ValueTask<FullOrderApiDTO> CreateOrderAsync(CreateOrderApiDTO createOrder)
        {
            if (!IsValidCreate(createOrder)) { throw new BadRequestException(); }
            var order = MapCreateOrderApiDTOToOrder(createOrder);
            OrderDTO orderDTO = null;
            try
            {
                orderDTO = await _context.Orders
                    .Where(x => x.Id == order.Id)
                    .FirstOrDefaultAsync();
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            if (orderDTO != null) { throw new ConflictException(); }

            try
            {
                order.Status = Infrastructure.OrderStatus.New;
                order.Created = DateTime.Now.ToUniversalTime();
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return MapOrderDtoToFull(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async ValueTask<FullOrderApiDTO> ChangeOrderAsync(Guid guid, ChangeOrderApiDTO changeOrder)
        {
            OrderDTO orderDTO = null;
            if (!IsValidChange(changeOrder)) { throw new BadRequestException(); }

            try
            {
                orderDTO = await _context.Orders
                    .Where(x => x.Id == guid)
                    .Include(X => X.Products)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            if (orderDTO != null)
            {
                switch (orderDTO.Status)
                {
                    case Infrastructure.OrderStatus.Completed:
                    case Infrastructure.OrderStatus.Delivered:
                    case Infrastructure.OrderStatus.HandedForDelivery:
                        throw new ForbiddenException();
                }
            }

            if (orderDTO != null)
            {
                orderDTO.Status = changeOrder.Status;
                orderDTO.Products.Clear();
                orderDTO.Products = changeOrder.Products
                    .Select(x=> MapProductApiDtoToProductDTO(x))
                    .ToList();
                _context.SaveChanges();

                return MapOrderDtoToFull(orderDTO);
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task DeleteOrderAsync(Guid guid)
        {
            OrderDTO orderDTO = null;

            try
            {
                orderDTO = await _context.Orders
                    .Where(x => x.Id == guid)
                    .Include(X => X.Products)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            if (orderDTO != null)
            {
                _context.Orders.Remove(orderDTO);
                await _context.SaveChangesAsync();
                return;
            }
            else
            {
                throw new NotFoundException();
            }
        }

        private bool IsValidChange(ChangeOrderApiDTO changeOrder)
        {
            bool isValid = true;
            if (changeOrder.Products.Count == 0)
            {
                return false;
            }

            changeOrder.Products.ForEach(x =>
            {
                if(x.Qty <= 0) { isValid = false; }
            });

            return isValid;
        }

        private bool IsValidCreate(CreateOrderApiDTO createOrder)
        {
            bool isValid = true;
            if (createOrder.Products.Count == 0)
            {
                return false;
            }

            createOrder.Products.ForEach(x =>
            {
                if (x.Qty <= 0) { isValid = false; }
            });

            return isValid;
        }

        private ProductDTO MapProductApiDtoToProductDTO(ProductApiDTO productApiDTO)
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<ProductApiDTO, ProductDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<ProductDTO>(productApiDTO);
        }

        private OrderDTO MapCreateOrderApiDTOToOrder(CreateOrderApiDTO createOrder)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateOrderApiDTO, OrderDTO>()
                    .ForMember(x => x.Products,
                               y => y.MapFrom(s => s.Products));
                cfg.CreateMap<ProductApiDTO, ProductDTO>();
            });

            var mapper = config.CreateMapper();

            return mapper.Map<OrderDTO>(createOrder);
        }

        private FullOrderApiDTO MapOrderDtoToFull(OrderDTO orderDTO)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, FullOrderApiDTO>()
                    .ForMember(x => x.Products,
                               y => y.MapFrom(s => s.Products));
                cfg.CreateMap<ProductDTO, ProductApiDTO>();
            });

            var mapper = config.CreateMapper();

            return mapper.Map<FullOrderApiDTO>(orderDTO);
        }
    }
}
