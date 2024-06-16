using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Vani.Services.Kafka;

public class KafkaService : IKafkaService
{
    private readonly IProducer<Null, string> _producer;
    public KafkaService(IOptions<ProducerConfig> config)
    {
        var configValue = config.Value;
        _producer =  new ProducerBuilder<Null, string>(configValue).Build();
    }

    public async Task<bool> ProduceMessageAsync<T>(string topic, T message)
    {
        var json = JsonConvert.SerializeObject(message);

        var response = await _producer.ProduceAsync(topic, new Message<Null, string>
        {
            Value = json
        });

        return response.Status == PersistenceStatus.Persisted;
    }
}
