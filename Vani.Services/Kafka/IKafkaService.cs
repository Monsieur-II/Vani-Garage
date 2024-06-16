namespace Vani.Services.Kafka;

public interface IKafkaService
{
    Task<bool> ProduceMessageAsync<T>(string topic, T message);
}
