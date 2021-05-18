using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NFinance.Application
{
    [ExcludeFromCodeCoverage]
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Scan(_ => _
           .FromAssemblies(
               typeof(GanhoApp).Assembly,
               typeof(GastoApp).Assembly,
               typeof(ClienteApp).Assembly,
               typeof(InvestimentoApp).Assembly,
               typeof(ResgateApp).Assembly,
               typeof(TelaInicialApp).Assembly
           )
           .AddClasses()
           .AsImplementedInterfaces());
            return services;
        }
    }
}
