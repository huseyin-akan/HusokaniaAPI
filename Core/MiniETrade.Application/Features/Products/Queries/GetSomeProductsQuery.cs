using MediatR;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Common.Abstractions.Repositories.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Queries;

//TODO-HUS buraya Include yaptığımız bir query yazıcaz.
public record GetSomeProductsQuery : PageableQuery, IRequest<GetSomeProductsQueryResponse>, ICachableRequest
{
    public string CacheKey => $"GetSomeProductQuery({Page},{Size})"; //It's important this value is unique

    public bool BypassCache => false; //If true response wont be brought from cache.

    public string? CacheGroupKey => "GetProducts";
}

public class GetSomeProductsQueryHandler : IRequestHandler<GetSomeProductsQuery, GetSomeProductsQueryResponse>
{
    private readonly IProductReadRepository _productReadRepository;
    public GetSomeProductsQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }
    public async Task<GetSomeProductsQueryResponse> Handle(GetSomeProductsQuery request, CancellationToken cancellationToken)
    {
        var result = _productReadRepository.GetAllAsync();

        //TODO-HUS yukarıya Skip ve Take yazılmış ama bunlara gerek yok. Pageable yapısına geçelim.

        return new GetSomeProductsQueryResponse
        {
            Products = result
        };
    }
}

public class GetSomeProductsQueryResponse
{
    public object Products { get; set; }
}
