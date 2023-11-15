using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Caching
{
    public class DistributedCachingOptions
    {
        public int AbsoluteExpirationMinutes { get; set; }
        public int SlidingExpirationMinutes { get; set; }
    }
}
