using MediatR;
using Microsoft.AspNetCore.Http;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.ProductImageFiles;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Common.Abstractions.Storage;
using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.ProductImageFiles.Commands;

public class UploadProductImageCommandRequest : IRequest<UploadProductImageCommandResponse>
{
    public Guid Id { get; set; }
}

public class UploadProductImageCommandRequestHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IStorageService _storageService;
    private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

    public UploadProductImageCommandRequestHandler(
        IProductReadRepository productReadRepository,
        IHttpContextAccessor contextAccessor,
        IStorageService storageService,
        IProductImageFileWriteRepository productImageFileWriteRepository)
    {
        _productReadRepository = productReadRepository;
        _contextAccessor = contextAccessor;
        _storageService = storageService;
        _productImageFileWriteRepository = productImageFileWriteRepository;
    }

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        var imagesToUpload = _contextAccessor.HttpContext?.Request.Form.Files;

        var result = await _storageService.UploadAsync("product-images", imagesToUpload);

        var product = await _productReadRepository.GetAsync(p => p.Id == request.Id);

        await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
        {
            FileName = r.fileName,
            Path = r.pathOrContainerName,
            Products = new List<Product>() { product }
        }).ToList());


        return new();
    }
}

public class UploadProductImageCommandResponse
{
}
