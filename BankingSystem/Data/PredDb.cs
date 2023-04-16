using BankingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankingSystem.Data
{
    public static class PredDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Users.Any())
            {
                Console.WriteLine("Seeding Data...");

                context.Users.AddRange(
                    new User() { Name = "Test 1", Accounts = new List<Account> { new Account { Balance = 1600 }, new Account {Balance = 600 } } }
                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data!");
            }
        }
    }
}
