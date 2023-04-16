using BankingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Balance)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .IsRequired();
        }
    }
}
