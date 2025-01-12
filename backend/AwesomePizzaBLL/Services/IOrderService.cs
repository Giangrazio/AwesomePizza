using GenericUnitOfWork.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwesomePizzaDAL.Entities;
using AwesomePizzaDAL.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AwesomePizzaBLL.Models;

namespace AwesomePizzaBLL.Services
{
    public interface IOrderService
    {
        Task<OrderModel> CreateOrderAsync(OrderModel? order);
        Task UpdateOrderStatusAsync(long orderId, OrderStatus status);
        Task<OrderModel?> GetOrderAsync(long orderId);
        Task<OrderModel?> GetOrderByCodeAsync(string orderCode);
        Task<List<OrderModel>> GetOrdersAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        public async Task<OrderModel> CreateOrderAsync(OrderModel? order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);

            _unitOfWork.GenericRepository<OrderRepository>().Add(orderEntity);

            await _unitOfWork.SaveChangesAsync();

            return await GetOrderAsync(orderEntity.Id) ?? throw new NullReferenceException("Order not created");
        }

        public async Task<OrderModel?> GetOrderAsync(long orderId)
        {
            return await _unitOfWork.GenericRepository<OrderRepository>()
                .GetQueryable(o => o.Id == orderId)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderModel?> GetOrderByCodeAsync(string orderCode)
        {
            return await _unitOfWork.GenericRepository<OrderRepository>()
                .GetQueryable(o => o.UniqueCode != null && o.UniqueCode.Equals(orderCode))
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderModel>> GetOrdersAsync()
        {
            return await _unitOfWork.GenericRepository<OrderRepository>()
               .GetQueryable()
               .Include(o => o.OrderProducts)
               .ThenInclude(op => op.Product)
               .ProjectTo<OrderModel>(_mapper.ConfigurationProvider)
               .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(long orderId, OrderStatus status)
        {
            var orderEntity = await _unitOfWork.GenericRepository<OrderRepository>()
                .GetQueryable(o => o.Id == orderId)
                .FirstOrDefaultAsync();

            if (orderEntity == null)
            {
                throw new NullReferenceException("Order not found");
            }

            orderEntity.Status = status;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
