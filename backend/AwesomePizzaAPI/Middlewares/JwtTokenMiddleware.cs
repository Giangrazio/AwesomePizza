using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using AwesomePizzaDAL;
using AwesomePizzaBLL.Structure;
using AwesomePizzaBLL.Services;

namespace AwesomePizzaAPI.Middlewares
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate next;
        public JwtTokenMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, IUserService userService)
        {

            context.Response.OnStarting(() =>
            {
                var identity = context.User.Identity as ClaimsIdentity;
                ITokenCryptoSettings crypto = context.RequestServices.GetService(typeof(ITokenCryptoSettings)) as ITokenCryptoSettings;
                if (identity.IsAuthenticated)
                {
                    var token = CreateTokenForIdentity(identity, crypto.XTokenSecretKey);
                    var id = long.Parse(identity.Claims.First(e => e.Type == "Id").Value);
                    context.Items["User"] = userService.GetUserById(id);
                    context.Response.Headers.Add("X-Token", token);
                }
                return Task.CompletedTask;
            });
            await next.Invoke(context);
        }

        //In questo metodo creiamo il token a partire dai claim della ClaimsIdentity
        private StringValues CreateTokenForIdentity(ClaimsIdentity identity, string SecretKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "http://awesomepizza.it",
                audience: "Audience",
                claims: identity.Claims,
                expires: DateTime.Now.AddDays(1),
                //AddMinutes(120),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            var serializedToken = tokenHandler.WriteToken(token);
            return serializedToken;
        }
    }
}
