using GenericUnitOfWork.UoW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AwesomePizzaDAL;
using AwesomePizzaDAL.Entities;
using AwesomePizzaDAL.Repositories;
using AwesomePizzaAPI;
using AwesomePizzaBLL.Models;
using AwesomePizzaBLL.Services;

namespace AwesomePizzaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("nickname-available/{nickname}")]
        public HttpMessage IsNicknameAvailable(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname)) return new HttpMessage() { success = false, message = "nickname can't be empty" };
            var result = _userService.IsNicknameAvailable(nickname);
            if (result)
            {
                return new HttpMessage() { success = true, message = "Ok" };
            }

            return new HttpMessage() { success = false, message = "Not available" };
        }

        [HttpGet("get-all")]
        public HttpMessage GetAll()
        {
            try
            {
                List<UserModel> users = _userService.GetAll();
                return new HttpMessage() { message = "Dati letti correttamente", success = true, data = users };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new HttpMessage() { message = "Errore lettura dati", success = false };
            }
        }
    }
}
