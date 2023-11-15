using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Caching
{
    public interface ICachingService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);
        Task AddCacheKeyToGroup(ICachableRequest request, CancellationToken cancellationToken);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}