using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

using SampleProjectBackEnd.Domain.Entities;
using SampleProjectBackEnd.Infrastructure.Identity;
using SampleProjectBackEnd.Infrastructure.Identity.Services;
using SampleProjectBackEnd.Infrastructure.Persistence;
using SampleProjectBackEnd.Infrastructure.Persistence.Repositories;
using SampleProjectBackEnd.Infrastructure.Token;
using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Application.Interfaces.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 🔹 DbContexts
        services.AddDbContext<PersistenceContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ✅ IdentityContext mutlaka eklenmeli
        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // 🔹 Identity
        services.AddIdentity<AppUser, IdentityRole<int>>(opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

        // 🔹 JWT
        services.Configure<JwtSettings>(options =>
            configuration.GetSection("JwtSettings").Bind(options));
        services.AddScoped<JwtTokenHelper>();
        services.AddScoped<IAuthService, IdentityService>();

        // 🔹 Repository - UnitOfWork
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
