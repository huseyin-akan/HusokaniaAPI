using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MiniETrade.Application.Common.Abstractions.Persistence.Dynamic;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories;
using MiniETrade.Application.Common.Abstractions.Repositories.Pagination;
using MiniETrade.Domain.Entities.Common;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence;

public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : BaseEntity
{
    private readonly BaseDbContext _context;

    public ReadRepository(BaseDbContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> Query() => _context.Set<TEntity>();

    public async Task<IEnumerable<TEntity?>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>,
    IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
    bool withDeleted = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking(); //böylece arka planda çalışan tracking sisteminin çalışmamasını sağlamış olduk.
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return orderBy(queryable);
        return queryable;
    }

    public async Task<Paginate<TEntity?>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>,
    IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
    int index = 0, int size = 100, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        var queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking(); //böylece arka planda çalışan tracking sisteminin çalışmamasını sağlamış olduk.
        if (include != null)
            queryable = include(queryable); 
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginationAsync(index, size, cancellation);
        return await queryable.ToPaginationAsync(index, size, cancellation);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
    bool withDeleted = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters(); //Eğer uygulanan bir QueryFilter var ise, ki biz mesela Product için DeletedDate üzerinden bir filter uyguladık onu ihmal eder.
        return await queryable.FirstOrDefaultAsync(predicate, cancellation);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        IQueryable<TEntity> query = Query();
        if (!enableTracking) query = query.AsNoTracking();
        if (withDeleted) query = query.IgnoreQueryFilters();
        if (predicate is not null) query = query.Where(predicate);

        return query.AnyAsync(cancellation);
    }

    public async Task<Paginate<TEntity?>> GetListByDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0, int size = 100, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellation = default)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPaginationAsync(index, size, cancellation);
    }
}

/* 
EF arka planda tracking yaparak dataların nasıl değiştiğini takip eder ve biz SaveChanges() dediğimizde bu takibe göre SQL oluşturur. Fakat read repository'de herhangi bir manipülasyon işlemi olmadığı için tracking'e gerek yoktur ve biz de onu kapattık.
*/