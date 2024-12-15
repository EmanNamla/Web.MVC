using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Core.Interfaces.Identity;
using Web.Infrastructure.Data;
using Web.Infrastructure.Data.DataSeed;

namespace Web.MVC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMvcLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
               
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                
            });


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpContextAccessor();
           
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var roleSeeder = serviceProvider.GetRequiredService<IServiceProvider>();
                SeedRoles.InitializeRoles(roleSeeder).Wait();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<ApplicationDbContext>();

                    SeedCategories.SeedAllCategories(context);
                }

            }
          

            return services;
        }

    }
}
