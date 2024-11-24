using Domain.Repositories;
using Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Data
{
    [ExcludeFromCodeCoverage]
    public static class InfraDataServicesExtensions
    {
        public static IServiceCollection AddInfraDataServices(this IServiceCollection services)
        {
            services.AddSingleton<IPagamentoRepository, PagamentoRepository>();
            return services;
        }
    }
}