using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = MiniETrade.Domain.Entities.File;

namespace MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Files;

public interface IFileReadRepository :IReadRepository<File>
{}