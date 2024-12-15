using Microsoft.Extensions.DependencyInjection;
using Web.Application.Interfaces;
using Web.Application.Services;


namespace Web.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddHttpContextAccessor();
        }
    }
}
