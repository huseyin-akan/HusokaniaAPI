using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Repositories.Pagination;

public abstract record PageableQuery
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 100;
}