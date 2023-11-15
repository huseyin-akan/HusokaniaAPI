using MiniETrade.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Entities;

public class Product :BaseEntity
{
    public string Name { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }

    public virtual ICollection<Order>? Orders { get; set; }
    public virtual ICollection<ProductImageFile>? ProductImages { get; set; }
}