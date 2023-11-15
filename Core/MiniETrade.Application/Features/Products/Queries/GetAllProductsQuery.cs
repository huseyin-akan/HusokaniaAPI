using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Common.Abstractions.Repositories.Pagination;
using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Queries;

public record GetAllProductsQuery : PageableQuery, IRequest<GetAllProductsResponse>
{}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, GetAllProductsResponse>
{
    private readonly IProductReadRepository _productReadRepository;
    public GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }
    public async Task<GetAllProductsResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _productReadRepository.GetAllAsync(cancellation: cancellationToken);
        
        //TODO-HUS maple.
        return new GetAllProductsResponse
        {
            
        };
    }
}

public record GetAllProductsResponse
{
    IEnumerable<GetAllProductsDto> Products { get; set; }

    public GetAllProductsResponse()
    {
        Products = new List<GetAllProductsDto>();
    }
}

public record GetAllProductsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public DateTime Created { get; set; }
}