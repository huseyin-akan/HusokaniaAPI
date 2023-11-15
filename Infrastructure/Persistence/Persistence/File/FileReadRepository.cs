using Application.Common.Abstractions.Persistence.Repositories.Files;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Domain.Entities.File;

namespace Persistence.Persistence;

public class FileReadRepository : ReadRepository<File>, IFileReadRepository
{
    public FileReadRepository(BaseDbContext context) : base(context)
    {
    }
}
