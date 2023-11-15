using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Abstractions.Persistence.Repositories.Products;

public interface IProductWriteRepository :IWriteRepository<Product>
{}
