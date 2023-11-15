using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Repositories.Pagination;

public class Paginate<T> 
{
    public Paginate()
    {
        Items = Array.Empty<T>();  
    }

    public int Size { get; set; }
    public int Index { get; set; }
    public int Count { get; set; }
    public int Page { get; set; }
    public IList<T> Items { get; set; }
    public bool HasNext => Index + 1 < Page;
    public bool HasPrevious => Index > 0;
}