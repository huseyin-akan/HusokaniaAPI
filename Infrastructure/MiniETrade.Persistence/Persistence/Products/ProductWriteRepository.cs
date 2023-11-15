using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Domain.Entities;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence.Products;

public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
{
    public ProductWriteRepository(BaseDbContext context) : base(context)
    {}
}
