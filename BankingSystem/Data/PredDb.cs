using BankingSystem.Models;
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
            if (!context.Users.Any())
            {
                Console.WriteLine("Seeding Data...");

                context.Users.AddRange(
                    new User() { Id = 1, Name = "Test 1", Accounts = new List<Account> { new Account { Id = 1, Balance = 1600 }, new Account { Id = 2, Balance = 600 } } }
                    );

                context.SaveChanges();
            }
        }
    }
}
