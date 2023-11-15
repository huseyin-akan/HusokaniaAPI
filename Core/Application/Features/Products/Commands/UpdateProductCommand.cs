using MediatR;
using Application.Common.Abstractions.Persistence.Repositories.Products;
using Application.Common.Abstractions.Transactions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands;

public record UpdateProductCommandRequest : IRequest<UpdateProductCommandResponse>, ITransactionalRequest
{
    public Guid Id{ get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
}


public class UpdateProductCommandRequestHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;

    public UpdateProductCommandRequestHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var productToUpdate = await _productReadRepository.GetAsync(p => p.Id == request.Id)
            ?? throw new BusinessException(AppMessages.ProductNotAvailable);

        var mappedProduct = new Product(); //TODO-HUS burada mapleme yapmalıyız.
        var result = _productWriteRepository.UpdateAsync(mappedProduct);  

        return new UpdateProductCommandResponse() { };
    }        
}

public record UpdateProductCommandResponse
{

}