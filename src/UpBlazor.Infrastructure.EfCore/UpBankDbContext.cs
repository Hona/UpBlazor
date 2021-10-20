using Microsoft.EntityFrameworkCore;
using UpBlazor.Core.Models;

namespace UpBlazor.Infrastructure.EfCore
{
    internal class UpBankDbContext : DbContext
    {
        public UpBankDbContext(DbContextOptions<UpBankDbContext> options) : base(options)
        {
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
    }
}
