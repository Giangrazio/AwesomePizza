using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericUnitOfWork.UoW;
using Microsoft.EntityFrameworkCore;
using AwesomePizzaDAL.Entities;
using AwesomePizzaDAL.Repositories;
using AwesomePizzaBLL.Models;

namespace AwesomePizzaBLL.Services
{
    public interface IUserService
    {
        bool IsNicknameAvailable(string nickname);
        UserEntity? GetUserById(long id);
        List<UserModel> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public bool IsNicknameAvailable(string nickname)
        {

            try
            {
                var nick = _unitOfWork.GenericRepository<UserRepository>().GetQueryable().Any()
                    ? _unitOfWork.GenericRepository<UserRepository>().GetQueryable(e => e.NickName.ToLower() == nickname.ToLower())
                    : null;
                if (nick == null || nick.Any()) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public UserEntity? GetUserById(long id)
        {
            return _unitOfWork.GenericRepository<UserRepository>().GetById(id);
        }

        public List<UserModel> GetAll()
        {
            var users = _unitOfWork.GenericRepository<UserRepository>()
                .GetQueryable()
                .AsNoTracking()
                //.ProjectTo<ProductModel>(_mapper.ConfigurationProvider, parameters)
                .ToList();

            if (users.Any())
            {
                var results = _mapper.Map<List<UserModel>>(users);

                return results;
            }

            return new List<UserModel>();
        }
    }
}
