using Domain.Entities;

namespace Domain.Entities;

public class ProductImageFile : File
{
    public virtual ICollection<Product>? Products { get; set; }
}