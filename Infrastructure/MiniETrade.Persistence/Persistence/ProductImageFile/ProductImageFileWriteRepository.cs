using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.ProductImageFiles;
using MiniETrade.Domain.Entities;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence.Persistence;

public class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, IProductImageFileWriteRepository
{
    public ProductImageFileWriteRepository(BaseDbContext context) : base(context)
    {

    }
}
