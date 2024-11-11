using Domain.Consumer;
using Domain.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.MessageBroker
{
    public static class InfraMessageBrokerExtension
    {
        public static IServiceCollection AddInfraMessageBrokerServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBrokerConsumer, MessageBrokerConsumer>();
            services.AddSingleton<IMessageBrokerProducer, MessageBrokerProducer>();
            return services;
        }
    }
}
