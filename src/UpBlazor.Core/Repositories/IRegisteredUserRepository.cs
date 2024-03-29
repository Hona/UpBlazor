﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UpBlazor.Core.Models;

namespace UpBlazor.Core.Repositories
{
    public interface IRegisteredUserRepository : IGenericRepository<RegisteredUser>
    {
        Task<RegisteredUser> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<RegisteredUser> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RegisteredUser>> GetAllByIdsAsync(CancellationToken cancellationToken = default, params string[] ids);
    }
}