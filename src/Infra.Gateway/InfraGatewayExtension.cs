using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Gateway
{
    public static class InfraGatewayExtension
    {
        public static IServiceCollection AddInfraGatewayServices(this IServiceCollection services)
        {
            services.AddSingleton<IPagamentoGatewayService, PagamentoGatewayService>();
            return services;
        }
    }
}
