using Microsoft.EntityFrameworkCore;
using UpBlazor.Core.Models;

namespace UpBlazor.Infrastructure.EfCore
{
    internal class UpBankDbContext : DbContext
    {
        public UpBankDbContext(DbContextOptions<UpBankDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UpUserToken> UpUserTokens { get; set; }
        public DbSet<TwoUp> TwoUps { get; set; }
        public DbSet<TwoUpRequest> TwoUpRequests { get; set; }
        public DbSet<NormalizedAggregate> NormalizedAggregates { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<RecurringExpense> RecurringExpenses { get; set; }
        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public DbSet<SavingsPlan> SavingsPlans {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>().OwnsOne(p => p.Money);
            modelBuilder.Entity<RecurringExpense>().OwnsOne(p => p.Money);
            modelBuilder.Entity<SavingsPlan>().OwnsOne(p => p.Amount);

            modelBuilder.Entity<NormalizedAggregate>(entity =>
            {
                entity.OwnsOne(p => p.Incomes);
                entity.OwnsOne(p => p.RecurringExpenses);
                entity.OwnsOne(p => p.SavingsPlans);

                entity.HasNoKey();
            });

            modelBuilder.Entity<TwoUp>().HasNoKey();
            modelBuilder.Entity<TwoUpRequest>().HasNoKey();

            //modelBuilder.Entity<UpUserToken>(entity =>
            //{
            //    entity.HasNoKey()
            //    entity.Property(x => x.UserId).
            //});
            //modelBuilder.Entity<UpUserToken>()
            //    .HasKey(p => p.Id);
        }
    }
}
