using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

using SampleProjectBackEnd.Infrastructure.Identity;
using SampleProjectBackEnd.Infrastructure.Persistence;
using SampleProjectBackEnd.Infrastructure.Token;

using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Application.Interfaces.Services;

using SampleProjectBackEnd.Infrastructure.Persistence.Repositories;
using SampleProjectBackEnd.Infrastructure.Identity.Services;
using SampleProjectBackEnd.Application.Services;


namespace SampleProjectBackEnd.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {//MSSQL
         //services.AddDbContext<ApplicationDbContext>(options =>
         //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionMSSQL")));


            //MYSQL
            services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        configuration.GetConnectionString("DefaultConnectionMYSQL"),
        ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnectionMYSQL"))
    ));


            // 🔹 Identity (ApplicationUser kullanıyoruz)
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            // 🔹 JWT
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<ITokenService, JwtTokenHelper>();


            // 🔹 Auth Service (IdentityService → Application IAuthService'i karşılar)
            services.AddScoped<IAuthService, IdentityService>();


            // 🔹 Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();


            // 🔹 Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            return services;
        }
    }
}
