using DevIO.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() //posso custumizar o comportamento do Identity, mas por enquanto vou utilizar o Identity como padrão
                .AddEntityFrameworkStores<AplicationDbContext>()
                .AddDefaultTokenProviders(); //devolve tokens que identicam originalidade. Pode ser usado em envio de email, reset de senha etc

            return services;

        }
    }
}
