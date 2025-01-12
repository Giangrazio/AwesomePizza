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
    public interface IMenuService
    {
        Task<ProductModel?> GetProductByIdAsync(int id);
        Task<List<ProductModel>> GetProductsAsync();
        Task<ProductModel> AddProductAsync(ProductModel product);
    }

    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWork context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;

        }

        public async Task<ProductModel> AddProductAsync(ProductModel product)
        {
            var productEntity = _mapper.Map<ProductEntity>(product);
            _unitOfWork.GenericRepository<ProductRepository>().Add(productEntity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductModel>(productEntity);
        }

        public async Task<ProductModel?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.GenericRepository<ProductRepository>()
                .GetQueryable(n => n.Id == id)
                .ProjectTo<ProductModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<List<ProductModel>> GetProductsAsync()
        {
            var products = await _unitOfWork.GenericRepository<ProductRepository>()
                 .GetQueryable()
                 .ProjectTo<ProductModel>(_mapper.ConfigurationProvider)
                 .ToListAsync();

            return products;
        }
    }
}
