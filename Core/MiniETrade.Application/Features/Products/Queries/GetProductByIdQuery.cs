using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Queries;

public class GetProductByIdQueryRequest : IRequest<GetProductByIdQueryResponse>
{
    public Guid Id { get; set; }
}

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
{
    private readonly IProductReadRepository _productReadRepository;

    public GetProductByIdQueryHandler(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }
    public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _productReadRepository.GetAsync(p => p.Id == request.Id);
        return new() 
        { 
            Id = result.Id,
            Name = result.Name,
            Price = result.Price,
            Stock = result.Stock,
            CreatedDate = result.Created
        };
    }
}

public class GetProductByIdQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
}