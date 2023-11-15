using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Files;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = MiniETrade.Domain.Entities.File;

namespace MiniETrade.Persistence.Persistence;

public class FileReadRepository : ReadRepository<File>, IFileReadRepository
{
    public FileReadRepository(BaseDbContext context) : base(context)
    {
    }
}
