using AwesomePizza.Tests.DatabaseSnapshot;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AwesomePizzaDAL;
using AwesomePizzaBLL;
using System.Reflection;
using GenericUnitOfWork.UoW;
using Microsoft.AspNetCore.Http;

namespace AwesomePizza.Tests.Common;
public static class FakeServerFactory
{
    public static DatabaseSnapshoter DbSnapshoter { get; private set; }

    public static WebApplicationFactory<Program> GetFakeServer()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    
                    var dbContext = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AwesomePizzaContext>));

                    if (dbContext != null)
                        services.Remove(dbContext);

                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    var dbName = Guid.NewGuid().ToString().Replace("-", "");

                    services.AddDbContext<AwesomePizzaContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                        options.UseInternalServiceProvider(serviceProvider);
                    });
                    var sp = services.BuildServiceProvider();

                    DbSnapshoter = new DatabaseSnapshoter(sp.GetService<AwesomePizzaContext>()!);

                    services.AddTransient<IUnitOfWork, UnitOfWork>(uow => new UnitOfWork(uow.GetRequiredService<AwesomePizzaContext>(), uow.GetRequiredService<IHttpContextAccessor>().HttpContext));
                    services.AddTransient<IInstallService, InstallService>();

                    using (var scope = sp.CreateScope())
                    {
                        using (var appContext = scope.ServiceProvider.GetRequiredService<AwesomePizzaContext>())
                        {
                            //AwesomePizzaContext.Initialize(appContext);
                            appContext.Database.EnsureCreated();

                            var installService = scope.ServiceProvider.GetRequiredService<IInstallService>();
                            installService.Initialize();
                        }

                        
                    }
                });
            });

        application.CreateClient();
        return application;
    }
}
