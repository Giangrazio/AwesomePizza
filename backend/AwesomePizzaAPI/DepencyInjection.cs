using System.Reflection;
using System.Text;
using GenericUnitOfWork.UoW;
using MagicCrypto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AwesomePizzaDAL;
using AwesomePizzaBLL;
using AwesomePizzaBLL.Structure;
using AwesomePizzaBLL.Mapping;
using AwesomePizzaBLL.Services;

namespace AwesomePizzaAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataRepository(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>(uow => new UnitOfWork(uow.GetRequiredService<AwesomePizzaContext>(), uow.GetRequiredService<IHttpContextAccessor>().HttpContext));

            return services;
        }

        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            string? connectionString = null;
            try
            {
                bool dockerExecution = bool.Parse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "false");
                connectionString = !dockerExecution
                    ? configuration.GetConnectionString("local")
                    : Environment.GetEnvironmentVariable("CONNECTIONSTRING");
                if (connectionString == null)
                {
                    throw new ArgumentNullException($"Impossibile trovare la Connection String {env.EnvironmentName}");
                }
                services.AddDbContext<AwesomePizzaContext>(o =>
                {
                    Console.WriteLine("Sql Server setup: done");
                    o.UseSqlServer(connectionString,
                        b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
                });
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddAutoMapper((sp, cfg) => cfg.AddProfile<MappingProfile>(), AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ITokenCryptoSettings, TokenCryptoSettings>();
            services.AddTransient<ICrypto, Crypto>();
            services.AddTransient<IInstallService, InstallService>();

            return services;
        }

        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    ITokenCryptoSettings crypto = new TokenCryptoSettings();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://awesomepizza.it",
                        ValidAudience = "Audience",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(crypto.XTokenSecretKey)
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            return services;
        }

        public static IApplicationBuilder InitializeContext(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AwesomePizzaContext>();
                AwesomePizzaContext.Initialize(context);
                //IUnitOfWork unitOfWork = services.GetRequiredService<IUnitOfWork>();

                var installService = services.GetRequiredService<IInstallService>();
                installService.Initialize();

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred while seeding the database.", ex);
                throw;
            }

            return app;
        }
    }
}
