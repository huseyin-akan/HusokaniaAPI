using Microsoft.EntityFrameworkCore;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Domain.Entities;
using MiniETrade.Infrastructure.Services;
using MiniETrade.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public BaseDbContext AppDbContext { get; private set; }
    private readonly ICurrentUserService currentUserService;

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<BaseDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        currentUserService = A.Fake<ICurrentUserService>();
        AppDbContext = new BaseDbContext(options, currentUserService);

        InitializeAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        AppDbContext.Database.EnsureDeleted(); // Delete the in-memory database after all tests
        AppDbContext.Dispose();
    }

    private async Task InitializeAsync()
    {
        await SeedProductData(AppDbContext);
    }

    private static async Task SeedProductData(BaseDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            context.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Computer",
                Price = 10,
                Stock = 30
            });

            context.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Laptop",
                Price = 20,
                Stock = 70
            });

            context.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Earphone",
                Price = 15,
                Stock = 130

            });
            await context.SaveChangesAsync();
        }
    }
}
