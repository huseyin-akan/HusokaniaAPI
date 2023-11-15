using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Entities.Events
{
    public record ProductCreated(
        Guid Id,
        DateTime CreatedDate,
        string Name,
        int Stock,
        float Price
     );
}
