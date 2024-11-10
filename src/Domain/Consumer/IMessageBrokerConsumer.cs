namespace Domain.Consumer
{
    public interface IMessageBrokerConsumer
    {
        public Task ReceiveMessageAsync();
    }
}
