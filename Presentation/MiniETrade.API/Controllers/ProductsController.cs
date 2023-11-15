using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Application.Features.ProductImageFiles.Commands;
using MiniETrade.Application.Features.Products.Commands;
using MiniETrade.Application.Features.Products.Commands.CreateProduct;
using MiniETrade.Application.Features.Products.Queries;
using MiniETrade.Domain.Exceptions;
using System.Net;

namespace MiniETrade.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes="Admin")]
public class ProductsController : ControllerBase
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;

    private readonly IMediator _mediator;

    public ProductsController(
        IProductReadRepository productReadRepository,
        IProductWriteRepository productWriteRepository,
        IMediator mediator)
    {
        _productReadRepository = productReadRepository;
        _productWriteRepository = productWriteRepository;
        _mediator = mediator;
    }

    [HttpGet("addsome")]
    public async Task<IActionResult> AddProducts()
    {
        //TODO-HUS
        //await _productWriteRepository.AddRangeAsync(
        //    new()
        //    {
        //        new() { Id = Guid.NewGuid(), Name = "Product1", Price = 100, Created = DateTime.UtcNow, Stock = 10 },
        //        new() { Id = Guid.NewGuid(), Name = "Product2", Price = 200, Created = DateTime.UtcNow, Stock = 20 },
        //        new() { Id = Guid.NewGuid(), Name = "Product3", Price = 300, Created = DateTime.UtcNow, Stock = 30 },
        //        new() { Id = Guid.NewGuid(), Name = "Product4", Price = 400, Created = DateTime.UtcNow, Stock = 40 },
        //        new() { Id = Guid.NewGuid(), Name = "Product5", Price = 500, Created = DateTime.UtcNow, Stock = 50 }
        //    }
        //    );
        //await _productWriteRepository.SaveAsync(); //TODO-HUS
        return Ok("Ekleme işlemi başarılı oldu");
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [Authorize("Admin"), HttpPost("addproduct")]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductCommand request)
    {
        await _mediator.Send(request);
        return StatusCode((int) HttpStatusCode.Created);
    }

    [HttpPut("updateproduct")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommandRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("getproductbyid/{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] GetProductByIdQueryRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpDelete("deleteproduct/{productId}")]
    public async Task<IActionResult> DeleteProduct( [FromRoute] DeleteProductCommandRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("uploadimage")]
    public async Task<IActionResult> UploadImage([FromQuery] UploadProductImageCommandRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> GetProductImages(string id)
    {
        var product = await _productReadRepository.Query().Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
       
        return Ok(product?.ProductImages.Select(p => new
        {
            p.Path,
            p.FileName,
            p.Id
        }));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProductImage(string productId, string imageId)
    {
        var product = await _productReadRepository.Query().Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId)) ?? throw new DataNotFoundException();
        
        
        var productImageFile = product.ProductImages.FirstOrDefault(p => p.Id == Guid.Parse(imageId)) ?? throw new DataNotFoundException();
        product.ProductImages.Remove(productImageFile);
        //await _productWriteRepository.SaveAsync(); TODO-HUS
        return Ok();
    }
}
