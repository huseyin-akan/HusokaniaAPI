using MediatR;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand : IRequest<CreateProductResponse>, ICacheRemoverRequest
    {
        public string Name { get; set; } = "";
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string? CacheKey => null;
        public bool BypassCache => false;
        public string? CacheGroupKey => "GetProducts";
    }

    public record CreateProductResponse(int? MyProperty);
}
