using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Domain.Entities.File;

namespace Application.Common.Abstractions.Persistence.Repositories.Files;

public interface IFileReadRepository :IReadRepository<File>
{}