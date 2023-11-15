using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MiniETrade.Application.Common.Abstractions.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Behaviours;

public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>, ICacheRemoverRequest
{
    private readonly ICachingService _cache;

    public CacheRemovingBehavior(ICachingService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request.BypassCache) return await next();

        TResponse response = await next();

        if (request.CacheGroupKey != null)
        {
            var cachedGroup = await _cache.GetAsync<HashSet<string>>(request.CacheGroupKey, cancellationToken);
            if (cachedGroup != null)
            {
                foreach (string key in cachedGroup)
                {
                    await _cache.RemoveAsync(key, cancellationToken);
                }

                await _cache.RemoveAsync(request.CacheGroupKey, cancellationToken);
            }
            return response;
        }

        if (request.CacheKey != null)
        {
            await _cache.RemoveAsync(request.CacheKey, cancellationToken);
        }
        return response;
    }
}