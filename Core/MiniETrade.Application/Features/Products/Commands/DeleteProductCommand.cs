using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Commands;

public class DeleteProductCommandRequest : IRequest<DeleteProductCommandResponse>
{
    public Guid ProductId { get; set; }
}

public class DeleteProductCommandRequestHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;

    public DeleteProductCommandRequestHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var productToDelete = await _productReadRepository.GetAsync(p => p.Id == request.ProductId)
            ?? throw new BusinessException(AppMessages.ProductNotAvailable);
        var result = await _productWriteRepository.DeleteAsync(productToDelete);
        return new();
    }
}

public class DeleteProductCommandResponse
{
}
