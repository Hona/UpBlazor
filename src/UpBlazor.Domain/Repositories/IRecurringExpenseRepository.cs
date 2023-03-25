﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Domain.Models;

namespace UpBlazor.Domain.Repositories
{
    public interface IRecurringExpenseRepository : IGenericRepository<RecurringExpense>
    {
        Task<RecurringExpense> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RecurringExpense>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}