using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NFinance.Domain.Interfaces.Repository;
using NFinance.Domain.Services;
using NFinance.Infra.Repository;

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
            services.AddTransient<IGanhoRepository, GanhoRepository>();
            services.AddTransient<IInvestimentoRepository, InvestimentoRepository>();
            services.AddTransient<IResgateRepository, ResgateRepository>();
            services.AddTransient<IGastoRepository, GastoRepository>();
            services.AddTransient<IRedisRepository,RedisRepository>();
            services.AddScoped<BaseDadosContext, BaseDadosContext>();
        }
    }
}