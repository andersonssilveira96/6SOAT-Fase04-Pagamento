using Domain.Producer;
using RabbitMQ.Client;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace Infra.MessageBroker
{
    public class MessageBrokerProducer : IMessageBrokerProducer
    {
        public async Task SendMessageAsync<T>(string queue, T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq-service"
            };

            var connection = await factory.CreateConnectionAsync();

            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue, exclusive: false);

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(message, options);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: queue, body: body);
        }
    }
}
