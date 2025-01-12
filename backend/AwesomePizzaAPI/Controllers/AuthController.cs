using System.Globalization;
using System.Security.Claims;
using GenericUnitOfWork.UoW;
using MagicCrypto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AwesomePizzaDAL;
using AwesomePizzaDAL.Entities;
using AwesomePizzaDAL.Repositories;
using AwesomePizzaAPI;
using AwesomePizzaBLL.Models;
using AwesomePizzaBLL.Structure;
using AwesomePizzaBLL.Services;

namespace AwesomePizzaAPI.Controllers
{
    //[EnableCors("Policy")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IUserService _userService;
        private readonly ICrypto _cryptor;
        private readonly string _secretSeed;
        public AuthController(ITokenCryptoSettings criptosettings, ICrypto crypto, IUnitOfWork context, IUserService userService)
        {
            _context = context;
            _userService = userService;
            _secretSeed = criptosettings.PwdAccountSeedKey;
            _cryptor = crypto;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<HttpMessage>> Login(TokenRequestModel tokenRequest)
        {
            var resultVerifyCredential = await VerifyCredentials(tokenRequest.NickName, tokenRequest.Password);
            if (!resultVerifyCredential.success)
            {
                return Unauthorized(resultVerifyCredential);
            }
            AuthToken tok = (AuthToken)resultVerifyCredential.data;
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(nameof(tok.Denomination), tok.Denomination ?? "", "string"));
            identity.AddClaim(new Claim(nameof(tok.NickName), tok.NickName, "string"));
            identity.AddClaim(new Claim(nameof(tok.UserName), tok.UserName, "string"));
            identity.AddClaim(new Claim(nameof(tok.Id), tok.Id.ToString(), "long"));
            identity.AddClaim(new Claim(nameof(tok.LastTokenId), tok.LastTokenId, "string"));
            identity.AddClaim(new Claim(nameof(tok.ReleaseDate), tok.ReleaseDate.ToString(CultureInfo.CurrentCulture), "DateTime"));
            identity.AddClaim(new Claim(nameof(tok.ExpirationDate), tok.ExpirationDate.ToString(CultureInfo.CurrentCulture), "DateTime"));


            HttpContext.User = new ClaimsPrincipal(identity);

            return Ok(resultVerifyCredential);
        }

        [AllowAnonymous]
        [HttpGet("login-implicit/{userCode}")]
        public async Task<ActionResult<HttpMessage>> LoginImplicit(string userCode)
        {
            var resultVerifyCredential = await VerifyCredentials(userCode);
            if (!resultVerifyCredential.success)
            {
                return Unauthorized(resultVerifyCredential);
            }
            AuthToken tok = (AuthToken)resultVerifyCredential.data;
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(nameof(tok.Denomination), tok.Denomination ?? "", "string"));
            identity.AddClaim(new Claim(nameof(tok.NickName), tok.NickName, "string"));
            identity.AddClaim(new Claim(nameof(tok.UserName), tok.UserName, "string"));
            identity.AddClaim(new Claim(nameof(tok.Id), tok.Id.ToString(), "long"));
            identity.AddClaim(new Claim(nameof(tok.LastTokenId), tok.LastTokenId, "string"));
            identity.AddClaim(new Claim(nameof(tok.ReleaseDate), tok.ReleaseDate.ToString(CultureInfo.CurrentCulture), "DateTime"));
            identity.AddClaim(new Claim(nameof(tok.ExpirationDate), tok.ExpirationDate.ToString(CultureInfo.CurrentCulture), "DateTime"));


            HttpContext.User = new ClaimsPrincipal(identity);

            return Ok(resultVerifyCredential);
        }

        private async Task<HttpMessage> VerifyCredentials(string nickname, string password)
        {
            UserEntity? user = await _context.GenericRepository<UserRepository>()
                .GetQueryable(e => e.NickName.ToLower() == nickname.ToLower())
                .FirstOrDefaultAsync();

            if (user != null)
            {
                string passStored = null;

                try
                {
                    passStored = _cryptor.DecryptStringAES(user.Password, _secretSeed);
                }
                catch (Exception error)
                {
                    return new HttpMessage() { success = false, message = "Password Decrypt Error: " + error.Message, data = error };
                }

                if (password != passStored)
                {
                    return new HttpMessage() { success = false, message = "Invalid Credential" };

                }


                var token = new AuthToken()
                {
                    Id = (int)user.Id,
                    NickName = nickname,
                    UserName = nickname,
                    ReleaseDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(1),
                    LastTokenId = Guid.NewGuid().ToString(),
                    Denomination = user.Denomination
                };

                return new HttpMessage() { success = true, message = "Authenticated Correctly", data = token };
            }
            return new HttpMessage() { success = false, message = "User Not Found" };
        }

        private async Task<HttpMessage> VerifyCredentials(string codeValue)
        {
            UserEntity? user = await _context.GenericRepository<UserRepository>()
                .GetQueryable(e => e.CodeValue.ToLower() == codeValue.ToLower())
                .FirstOrDefaultAsync();

            if (user != null)
            {
                //string passStored = null;

                //try
                //{
                //    passStored = Cryptor.DecryptStringAES(user.Password, secretSeed);
                //}
                //catch (Exception error)
                //{
                //    return new HttpMessage() { success = false, message = "Password Decrypt Error: " + error.Message, data = error };
                //}

                //if (password != passStored)
                //{
                //    return new HttpMessage() { success = false, message = "Invalid Credential" };

                //}


                var token = new AuthToken()
                {
                    Id = (int)user.Id,
                    NickName = user.NickName,
                    UserName = user.NickName,
                    ReleaseDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(1),
                    LastTokenId = Guid.NewGuid().ToString(),
                    Denomination = user.Denomination,
                };

                return new HttpMessage() { success = true, message = "Authenticated Correctly", data = token };
            }
            return new HttpMessage() { success = false, message = "User Not Found" };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<HttpMessage>> Register(AccountRegisterModel newReg)
        {
            if (ModelState.IsValid)
            {
                IList<UserEntity> lsAcc = new List<UserEntity>();
                try
                {
                    lsAcc = await _context.GenericRepository<UserRepository>().GetQueryable(e => e.NickName == newReg.NickName).ToListAsync();
                    if (lsAcc.Any()) return new HttpMessage() { message = "Nickname già presente, esegui il login!", success = false };
                }
                catch (Exception error)
                {
                    return new HttpMessage() { message = $"Errore verifica Nickname: {newReg.NickName} - {error.Message} - {error.InnerException}", success = false };
                }

                var acc = new UserEntity()
                {
                    NickName = newReg.NickName,
                    RoleName = newReg.RoleName,
                    Password = _cryptor.EncryptStringAES(newReg.Password.Trim(), _secretSeed)
                };
                try
                {
                    _context.GenericRepository<UserRepository>().Add(acc);
                    await _context.SaveChangesAsync();
                }
                catch (Exception errore)
                {
                    return new HttpMessage() { message = $"Errore inserimento nuovo account: {errore.Message} - {errore.InnerException}", success = false };
                }

                return new HttpMessage() { message = "RegisterConfirm", success = true, data = new { nickname = acc.NickName, accountId = acc.Id } };
            }

            return new HttpMessage() { message = "Dati non validi", success = false };
        }


    }

}
