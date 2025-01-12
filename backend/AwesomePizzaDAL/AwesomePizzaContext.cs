using EFCore.AutomaticMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using AwesomePizzaDAL.Entities;

namespace AwesomePizzaDAL
{
    public sealed class AwesomePizzaContext : DbContext
    {

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderProductEntity> OrderProiducs { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }

        public AwesomePizzaContext(DbContextOptions<AwesomePizzaContext> options) : base(options)
        {
        }

        private static readonly Object SyncObj = new Object();
        public static bool Initialize(AwesomePizzaContext context)
        {
            lock (SyncObj)
            {
                if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory") return false;
                try
                {
                    var options = new DbMigrationsOptions()
                    {
                        AutomaticMigrationsEnabled = true,
                        AutomaticMigrationDataLossAllowed = false, // set false of default
                        ResetDatabaseSchema = false // set false of default
                    };
                    MigrateDatabaseToLatestVersion.Execute(context, options);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while seeding the database.", ex);
                    throw;
                }
            }

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTime>()
                .HaveConversion(typeof(UtcValueConverter));
        }

        class UtcValueConverter : ValueConverter<DateTime, DateTime>
        {
            public UtcValueConverter()
                : base(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("Context non configurato");
            }
        }
    }
}
