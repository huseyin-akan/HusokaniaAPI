using MiniETrade.Application.BusinessRules.Products;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Domain.Entities;
using MiniETrade.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.BusinessRules.Products;

public class ProductBusinessRulesTests
{
    private readonly ProductBusinessRules _productBusinessRules;
    private readonly IProductReadRepository _productReadRepository;
    public ProductBusinessRulesTests()
    {
        _productReadRepository = A.Fake<IProductReadRepository>();
        _productBusinessRules = new(_productReadRepository);
    }

    [Fact]
    public async Task ShouldThrowBusinessExceptionIfProductNameWasTaken()
    {
        //Arrange
        var productName = "ProductNameToTest";
        Product? productToReturn = new() { Id = Guid.NewGuid() };
        //A.CallTo(() => _productReadRepository.GetAsync(A<Expression<Func<Product, bool>>>._))!.Returns(Task.FromResult(productToReturn));
        //TODO-HUS yukarıyı Moq kütüphanesine geçirelim.

        //Act and Assert
        await Assert.ThrowsAsync<BusinessException>(async () => await _productBusinessRules.CheckIfProductNameIsDuplicate(productName)); 
    }

    [Fact]
    public async Task ShouldBeOkIfProductNameWasNotTaken()
    {
        //Arrange
        var productName = "ProductNameToTest";
        Product? product = null;
        //A.CallTo(() => _productReadRepository.GetAsync(A<Expression<Func<Product, bool>>>._)).Returns(Task.FromResult(product));
        //TODO-HUS yukarıyı Moq kütüphanesine geçirelim.

        //Act and Assert
        await FluentActions.Invoking(async () => await _productBusinessRules.CheckIfProductNameIsDuplicate(productName))
        .Should().NotThrowAsync();
    }
}