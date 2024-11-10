using Application.DTOs.Pagamentos;
using Application.UseCase.Pagamentos;
using Domain.Consumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;
using System.Threading.Channels;

namespace Infra.MessageBroker
{
    public class MessageBrokerConsumer : IMessageBrokerConsumer
    {
        private readonly IPagamentoUseCase _pagamentoUseCase;
        private IConnection _connection;
        private IChannel _channel;

        public MessageBrokerConsumer(IPagamentoUseCase pagamentoUseCase)
        {
            _pagamentoUseCase = pagamentoUseCase;
        }

        public async Task ReceiveMessageAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync("novos-pedidos", exclusive: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, eventArgs) => {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Order message received: {message}");
                await _pagamentoUseCase.GerarPagamento(new GerarPagamentoDto());
            };

            await _channel.BasicConsumeAsync(queue: "novos-pedidos", autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
        }
    }
}
