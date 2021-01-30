using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Infra.Repository;
using System.Diagnostics.CodeAnalysis;

namespace NFinance.Infra
{
    [ExcludeFromCodeCoverage]
    public static class ServicesExtensions
    {
        public static void AddInfraDataSqlServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BaseDadosContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BancoDeDados")).EnableSensitiveDataLogging());

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IInvestimentosRepository, InvestimentosRepository>();
            services.AddTransient<IResgateRepository, ResgateRepository>();
            services.AddTransient<IGastosRepository, GastosRepository>();
            services.AddScoped<BaseDadosContext, BaseDadosContext>();
        }
    }
}
