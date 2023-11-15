using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Entities
{
    public class ProductImageFile : File
    {
        public virtual ICollection<Product>? Products { get; set; }
    }
}
