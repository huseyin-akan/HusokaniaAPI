using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Customers;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Files;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Orders;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.ProductImageFiles;
using MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Products;
using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Persistence.Contexts;
using MiniETrade.Persistence.Persistence;
using MiniETrade.Persistence.Persistence.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Persistence;

public static class PersistenceServiceRegistration
{    
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Postgre SQL için
        services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:PostgreSQL"]));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //Postgre SQL DateTime Objesi için

        //Local MSSQL için
        //services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(configuration["ConnectionStrings:LocalMSSQL"]));

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<BaseDbContext>();

        services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
        services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
        services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
        services.AddScoped<IOrderReadRepository, OrderReadRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
        services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
        services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();
        services.AddScoped<IFileReadRepository, FileReadRepository>();
        services.AddScoped<IFileWriteRepository, FileWriteRepository>();
    }
}
