using MiniETrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Persistence.Repositories;

public interface IWriteRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);
    Task<TEntity> DeleteAsync(TEntity entity, bool deletePermanently = false);
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool deletePermanently = false);
}
