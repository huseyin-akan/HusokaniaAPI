using MiniETrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
