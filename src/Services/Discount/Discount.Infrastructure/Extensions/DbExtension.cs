using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDataBase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var config = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Discount DB Migration started");
                    ApplyMigrations(config);
                    logger.LogInformation("Discount DB Migration completed");


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return host;
        }

        private static void ApplyMigrations(IConfiguration config)
        {
            var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();
            using var cmd = new NpgsqlCommand()
            {
                Connection = connection
            };

            cmd.CommandText = "DROP TABLE IF EXISTS Coupon";
            cmd.ExecuteNonQuery();
            cmd.CommandText = @"CREATE TABLE Coupon(
                                    Id SERIAL PRIMARY KEY,
                                    ProductName VARCHAR(500) NOT NULL,
                                    Description TEXT,
                                    AMOUNT INT)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Adidas Quick Force Indoor Badminton Shoes','Shoe Discount',500);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)','Racquet Discount',700);";
            cmd.ExecuteNonQuery();

        }
    }
}
