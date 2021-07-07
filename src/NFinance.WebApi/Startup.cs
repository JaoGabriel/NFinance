using NSwag;
using System.Text;
using System.Linq;
using NFinance.Infra;
using NFinance.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using NSwag.Generation.Processors.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NFinance.Domain.ObjetosDeValor;
using NFinance.Infra.Options;
using NFinance.WebApi.Middleware;

namespace NFinance.WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            services.AddOpenApiDocument(c =>
            {
                c.Title = "Nfinance.WebApi";
                c.DocumentName = "Nfinance.WebApi";
                c.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Insira aqui o token: Bearer {seu JWT token}."
                });

                c.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
            services.AddInfraServices(Configuration);
            services.AddApplicationServices();

            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<TokenSettingsOptions>(Configuration.GetSection("TokenSettings"));
            
            services.AddControllers();

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenSettings:TokenSecret").Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    SaveSigninToken = true,
                    ValidAudience = "NFinance",
                    ValidIssuer = "NFinance"
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseOpenApi(c => c.DocumentName = "Nfinance.WebApi");
            app.UseSwaggerUi3();

            app.UseMiddleware(typeof(UsuarioInfoMiddleware));
            
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}