using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NFinance.Application.Interfaces;

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
        }
    }
}
