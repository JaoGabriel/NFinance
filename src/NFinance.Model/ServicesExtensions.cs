using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NFinance.Model.Mapper;

namespace NFinance.Model
{
    [ExcludeFromCodeCoverage]
    public static class ServicesExtensions
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new GastosProfile());
                cfg.AddProfile(new InvestimentosProfile());
                cfg.AddProfile(new ClienteProfile());
                cfg.AddProfile(new ResgateProfile());
                cfg.AddProfile(new PainelDeControleProfile());
            }).CreateMapper());

            return services;
        }
    }
}