using Microsoft.Extensions.DependencyInjection;
using Web.Core.Interfaces.Identity;
using Web.Core.Interfaces;
using Web.Infrastructure.Identity;
using Web.Infrastructure.Repositories;

namespace Web.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           // services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        }
    }
}
