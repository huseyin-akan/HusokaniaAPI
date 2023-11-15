using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Dynamic;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Common.Abstractions.Repositories.Pagination;
using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Queries
{
    public record GetProductsDynamicallyQuery : PageableQuery, IRequest<GetProductsDynamicallyQueryResponse>
    {
        public DynamicQuery? DynamicQuery { get; set; }
    }

    public class GetProductsDynamicallyQueryHandler : IRequestHandler<GetProductsDynamicallyQuery, GetProductsDynamicallyQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        public GetProductsDynamicallyQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }
        public async Task<GetProductsDynamicallyQueryResponse> Handle(GetProductsDynamicallyQuery request, CancellationToken cancellationToken)
        {
            Paginate<Product?> products = await _productReadRepository.GetListByDynamicAsync(
                dynamic: request.DynamicQuery,
                //include: p => p.Include(m => m.) //TODO-HUS EF eklemek gerek sanırım
                index : request.Page,
                size : request.Size
                );

            var result = _productReadRepository.GetAllAsync();

            return new GetProductsDynamicallyQueryResponse
            {
                Products = result
            };
        }
    }

    public class GetProductsDynamicallyQueryResponse
    {
        public object Products { get; set; }
    }
}