using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories;
using MiniETrade.Domain.Entities.Common;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence;

public class WriteRepository<TEntity> : IWriteRepository<TEntity> where TEntity : BaseEntity
{
    private readonly BaseDbContext _context;

    public WriteRepository(BaseDbContext context)
    {
        _context = context;
    }
    public IQueryable<TEntity> Query() => _context.Set<TEntity>();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity; //Burada DB'ye eklenen entityi döndürmüş oluyoruz.
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        await _context.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool deletePermanently = false)
    {
        await SetEntityAsDeletedAsync(entity, deletePermanently);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool deletePermanently = false)
    {
        await SetEntityAsDeletedAsync(entities, deletePermanently);
        await _context.SaveChangesAsync();
        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.LastModified = DateTime.UtcNow;
        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        foreach (TEntity entity in entities)
            entity.LastModified = DateTime.UtcNow;
        _context.UpdateRange(entities);
        await _context.SaveChangesAsync();
        return entities;
    }

    protected async Task SetEntityAsDeletedAsync(TEntity entity, bool deletePermanently)
    {
        if (deletePermanently)
            _context.Remove(entity);

        if (!deletePermanently)
        {
            CheckIfEntityHasOneToOneRelation(entity);
            await SetEntityAsSoftDeletedAsync(entity);
        }
    }

    protected void CheckIfEntityHasOneToOneRelation(TEntity entity)
    {
        bool hasEntityHaveOneToOneRelation = _context
            .Entry(entity)
            .Metadata.GetForeignKeys()
            .All(
            x =>
                x.DependentToPrincipal?.IsCollection == true
                || x.PrincipalToDependent?.IsCollection == true
                || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
            ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException(
                "Entity has one-to-one relationship. Soft delete causes problems if you try to entry again by some foreigner");
    }

    private async Task SetEntityAsSoftDeletedAsync(BaseEntity entity)
    {
        if (entity.Deleted.HasValue) return;
        entity.Deleted = DateTime.UtcNow;

        var navigations = _context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade })
            .ToList();
        foreach (INavigation? navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object? navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = _context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                    if (navValue == null)
                        continue;
                }

                foreach (BaseEntity navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = _context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((BaseEntity)navValue);
            }
        }

        _context.Update(entity);
    }

    protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        Type queryProviderType = query.Provider.GetType();
        MethodInfo createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: new object[] { query.Expression })!;
        return queryProviderQuery.Where(x => !((BaseEntity)x).Deleted.HasValue);
    }

    protected async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
    {
        foreach (TEntity entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
    }
}
