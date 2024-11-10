using Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.MessageBroker
{
    public static class InfraMessageBrokerExtension
    {
        public static IServiceCollection AddInfraMessageBrokerServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBrokerConsumer, MessageBrokerConsumer>();
            return services;
        }
    }
}
