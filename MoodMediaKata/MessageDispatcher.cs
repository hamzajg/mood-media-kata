using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;

namespace MoodMediaKata;

public class MessageDispatcher(IServiceProvider provider) : IAutoSubscriberMessageDispatcher
{
    public void Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken = new CancellationToken()) where TMessage : class where TConsumer : class, IConsume<TMessage>
    {
        using var scope = provider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
        consumer.Consume(message, cancellationToken);
    }

    public async Task DispatchAsync<TMessage, TConsumer>(TMessage message,
        CancellationToken cancellationToken = new CancellationToken()) where TMessage : class where TConsumer : class, IConsumeAsync<TMessage>
    {
        using var scope = provider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<TConsumer>();
        await consumer.ConsumeAsync(message, cancellationToken);
    }
}