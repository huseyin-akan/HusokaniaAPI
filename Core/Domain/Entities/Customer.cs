using Domain.Entities.Common;
using Domain.Entities;

namespace Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<Order>? Orders { get; set; }
}