using DevIO.Api.Data;
using DevIO.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders(); //devolve tokens que identicam originalidade. Pode ser usado em envio de email, reset de senha etc


            /*configuração de Jason Web Token (JWT)*/

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection); //capturar os dados da appsettings.json

            var appSettings = appSettingsSection.Get<AppSettings>(); //capturar o conteúdo do nó "AppSettings"
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                /* O padrão de autenticação é via token */
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                /* O padrão de funcionamento é via token */
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 

            }).AddJwtBearer(x =>
            {
                /* A aplicação só aceita autenticação de aplicações em https. 
                 * Evita ataques */
                x.RequireHttpsMetadata = true; 
                
                /* Guarda o token no httpsAutenticationProperties após a autenticação. 
                 * Fica mais fácil da aplicação validar o usuário loago apos a apresentação do token*/
                x.SaveToken = true;


                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //valida quem foi emitido, baseado na chave que foi passada
                    IssuerSigningKey = new SymmetricSecurityKey(key), //faz a configuração da chave para chave criptografia
                    ValidateIssuer = true, //informa se a verificação de Issuer é feita
                    ValidateAudience = true, //informa se a verificação de audiencia é feita
                    ValidAudience = appSettings.ValidoEm, //diz de onde vem a audiencia do token
                    ValidIssuer = appSettings.Emissor //diz de onde vem o Emissor do token 
                };

                
            });




            return services;

        }
    }
}
