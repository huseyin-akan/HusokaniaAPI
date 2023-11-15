using Application.UnitTests.Fixtures;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Domain.Entities;
using MiniETrade.Persistence.Persistence.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Common.Abstractions.Repositories;

public class ProductRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly Guid _testProductId;

    public ProductRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _productWriteRepository = new ProductWriteRepository(_fixture.AppDbContext);
        _testProductId = new Guid("a9a2b1ce-ecd8-4828-b00b-94c6574f8fa7"); 
        _productReadRepository = new ProductReadRepository(_fixture.AppDbContext);
    }

    [Fact]
    public async Task AddProductToDatabase()
    {
        Product productToAdd = new()
        {
            Id = _testProductId,
            Created = DateTime.Now,
            Name = "Mouse",
            Price = 12,
            Stock = 33
        };

        var result = await _productWriteRepository.AddAsync(productToAdd);

        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();
    }

    [Fact]
    public async Task GetProductById()
    {
        var result = await _productReadRepository.GetAsync(p => p.Id == _testProductId);
        result.Should().NotBeNull();
        result.Should().BeOfType<Product>();
        result.Name.Should().Be("Mouse");
        result.Price.Should().Be(12);
        result.Stock.Should().Be(33);
        result.Status.Should().BeTrue();
    }
}