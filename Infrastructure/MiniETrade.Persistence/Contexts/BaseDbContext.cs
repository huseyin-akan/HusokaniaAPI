using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Domain.Entities;
using MiniETrade.Domain.Entities.Common;
using MiniETrade.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using File = MiniETrade.Domain.Entities.File;

namespace MiniETrade.Persistence.Contexts;

public class BaseDbContext :IdentityDbContext<AppUser, AppRole, Guid>
{
    private readonly ICurrentUserService _currentUserService;
    public BaseDbContext(DbContextOptions options, ICurrentUserService currentUserService) : base(options)
    {
        //Database.EnsureCreated(); //TODO-HUS bu ne işe yarıyor bakalım. Migration yaparken hataya sebebiyet veriyor.
        _currentUserService = currentUserService;
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<ProductImageFile> ProductImageFiles { get; set; }
    public DbSet<InvoiceFile> InvoiceFiles { get; set; }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var datas = ChangeTracker.Entries<BaseEntity>();

        foreach (var data in datas)
        {
            switch (data.State)
            {
                case EntityState.Added:
                    data.Entity.CreatedBy = _currentUserService.UserId;
                    data.Entity.Created = DateTime.Now;
                    data.Entity.Status = true;
                    data.Entity.Id = Guid.NewGuid();
                    break;

                case EntityState.Modified:
                    data.Entity.LastModifiedBy = _currentUserService.UserId;
                    data.Entity.LastModified = DateTime.Now;
                    break;
            }
        }
            
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //Assembly'de IEntityTypeConfiguration<T> interface'ini implement eden configuration classlarını ekler.

        base.OnModelCreating(builder);
    }
}