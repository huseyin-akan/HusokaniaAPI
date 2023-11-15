using MediatR;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        //private readonly IMassTransitService _massTransitService; TODO-HUS geçici olarak kapattık. Açılmalı

        public CreateProductCommandHandler(
            IProductWriteRepository productWriteRepository
            //,IMassTransitService massTransitService
            )
        {
            _productWriteRepository = productWriteRepository;
            //_massTransitService = massTransitService;
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var addedProduct = await _productWriteRepository.AddAsync(new() //TODO-HUS Mapping.
            {
                Name = request.Name,
                Stock = request.Stock,
                Price = request.Price,

            });

            //var productCreatedEvent = new ProductCreated(addedProduct.Id, addedProduct.CreatedDate, addedProduct.Name, addedProduct.Stock, addedProduct.Price);
            //await _massTransitService.Publish(productCreatedEvent); //Ürün ekleme ile ilgili event fırlattık. TODO-HUS geçici olarak kapattık.

            return new CreateProductResponse(null) { }; //TODO-HUS recordlarda parametre isteyince zorunlu constructor yapıyor. Boş constructor için düşünelim.
        }
    }
}
