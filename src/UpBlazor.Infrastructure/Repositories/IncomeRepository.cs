using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using UpBlazor.Core.Models;
using UpBlazor.Core.Repositories;

namespace UpBlazor.Infrastructure.Repositories
{
    public class IncomeRepository : GenericRepository<Income>, IIncomeRepository
    {
        public IncomeRepository(IDocumentStore store) : base(store) { }

        public async Task<Income> GetByIdAsync(Guid id)
        {
            await using var session = Store.QuerySession();

            return await session.Query<Income>()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Income>> GetAllByUserIdAsync(string userId)
        {
            await using var session = Store.QuerySession();

            var output = await session.Query<Income>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
            
            var duplicateIncomes = output.GroupBy(x => x.Name);

            foreach (var duplicateIncomeGrouping in duplicateIncomes)
            {
                var duplicateIncomeGroupingList = duplicateIncomeGrouping.ToList();
                for (var i = 0; i < duplicateIncomeGroupingList.Count; i++)
                {
                    if (i == 0)
                    {
                        continue;
                    }

                    var duplicateIncome = duplicateIncomeGroupingList[i];

                    duplicateIncome.Name += " #" + i;
                }
            }

            return output;
        }
    }
}