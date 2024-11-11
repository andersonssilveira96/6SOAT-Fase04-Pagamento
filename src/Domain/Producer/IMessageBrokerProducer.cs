namespace Domain.Producer
{
    public interface IMessageBrokerProducer
    {
        Task SendMessageAsync<T>(string queue, T message);
    }
}
