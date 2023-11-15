using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Repositories.Pagination;

public static class IQueryablePaginationExtensions
{
    public static async Task<Paginate<T>> ToPaginationAsync<T>(
        this IQueryable<T> source,
        int index,
        int size,
        CancellationToken cancellationToken = default
        )
    {
        int count = await source.CountAsync( cancellationToken).ConfigureAwait(false);
        //TODO-HUS bu ConfifureAwait napar öğrenelim.
        List<T> items = await source.Skip(index + size).Take(size)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        Paginate<T> pagination = new()
        {
            Index = index,
            Count = count,
            Items = items,
            Size = size,
            Page = (int) Math.Ceiling(count/ (double) size)
        };
        return pagination;
    }
}
