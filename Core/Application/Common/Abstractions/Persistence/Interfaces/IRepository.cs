using Microsoft.EntityFrameworkCore;
using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Abstractions.Persistence.Repositories;

public interface IRepository<TEntity>  where TEntity: BaseEntity
{
    IQueryable<TEntity> Query();
}
