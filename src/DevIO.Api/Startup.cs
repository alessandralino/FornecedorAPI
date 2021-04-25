using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevIO.Api.Configuration;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentityConfiguration(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.WebApiConfig();

            services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production");
                app.UseHsts(); 
                /* Uma vez que a aplicação foi utilizada atráves de um browser ela vai responder dizendo 'eu entendo https'
                 * O Broser vai guardar o cache em https. No entanto o hsts só funciona se for feita uma cominicação HTTPS. 
                 */
            }

            app.UseAuthentication();
            app.UseMvcConfiguration();
        }
    }
}
