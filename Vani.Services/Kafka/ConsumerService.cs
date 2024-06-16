using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Vani.Shared;

namespace Vani.Services.Kafka;

public class ConsumerService : BackgroundService, IConsumerService
{
    private readonly IConsumer<Null, string> _consumer;
    
    public ConsumerService(IOptions<ConsumerConfig> config)
    {
        var configValue = config.Value;
        _consumer = new ConsumerBuilder<Null, string>(configValue).Build();
        _consumer.Subscribe(Constants.GarageTopic);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    Console.WriteLine(
                        $"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured: {e.Message}");
            }
        }, stoppingToken);
    }
    
    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}
