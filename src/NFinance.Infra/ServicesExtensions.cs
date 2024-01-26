using System;
using NFinance.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NFinance.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using NFinance.Domain.Identidade;

namespace NFinance.Infra
{
    [ExcludeFromCodeCoverage]
    public static class ServicesExtensions
    {
        public static void AddInfraServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BaseDadosContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("BancoDeDados")).EnableSensitiveDataLogging());

            services.AddIdentity<Usuario, Role>().AddEntityFrameworkStores<BaseDadosContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //Claims
                options.Tokens.AuthenticatorIssuer = "NFinance";

                //Login
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.SlidingExpiration = true;
            });

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IGanhoRepository, GanhoRepository>();
            services.AddTransient<IInvestimentoRepository, InvestimentoRepository>();
            services.AddTransient<IResgateRepository, ResgateRepository>();
            services.AddTransient<IGastoRepository, GastoRepository>();
            services.AddScoped<BaseDadosContext, BaseDadosContext>();
        }
    }
}