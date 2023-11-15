using Domain.Entities.Common;
using Domain.Entities;

namespace Domain.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }

    public virtual ICollection<Product>? Products { get; set; }
    public virtual Customer? Customer { get; set; }
}