using System.Diagnostics.CodeAnalysis;
using NFinance.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NFinance.Application
{
    [ExcludeFromCodeCoverage]
    public static class ServicesExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IClienteApp, ClienteApp>();
            services.AddTransient<IGanhoApp, GanhoApp>();
            services.AddTransient<IInvestimentoApp, InvestimentoApp>();
            services.AddTransient<IGastoApp, GastoApp>();
            services.AddTransient<IResgateApp, ResgateApp>();
            services.AddTransient<ITelaInicialApp, TelaInicialApp>();
            services.AddTransient<IRedisApp, RedisApp>();
        }
    }
}
