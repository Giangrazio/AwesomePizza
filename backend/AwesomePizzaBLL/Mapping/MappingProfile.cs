using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using AwesomePizzaDAL.Entities;
using AwesomePizzaBLL.Models;

namespace AwesomePizzaBLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductEntity, ProductModel>()
                .ReverseMap();

            CreateMap<OrderProductEntity, OrderProductModel>()
                .ReverseMap();

            CreateMap<OrderEntity, OrderModel>()
                .ReverseMap();

            CreateMap<UserEntity, UserModel>()
                .ReverseMap();
        }
    }
}
