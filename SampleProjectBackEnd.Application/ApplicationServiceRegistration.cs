using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SampleProjectBackEnd.Application.Interfaces.Services;
using SampleProjectBackEnd.Application.Services;
using SampleProjectBackEnd.Application.Validators;
using System.Reflection;

namespace SampleProjectBackEnd.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
