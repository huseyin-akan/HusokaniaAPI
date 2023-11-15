using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Caching;

public interface ICachableRequest
{
    string CacheKey { get; }
    bool BypassCache { get; }
    string? CacheGroupKey { get; }
}