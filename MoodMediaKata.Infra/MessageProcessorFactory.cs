namespace MoodMediaKata.Infra;

public class MessageProcessorFactory
{
    public static IMessageProcessor CreateMessageProcessor(string messageProcessorType)
    {
        switch (messageProcessorType)
        {
            case "QueueBus":
                return new QueueBusMessageProcessor();
            case "Console":
                return new ConsoleMessageProcessor();
            default:
                throw new ArgumentException("Invalid message processor type", nameof(messageProcessorType));
        }
    }
}

public class ConsoleMessageProcessor : IMessageProcessor
{
}

public class QueueBusMessageProcessor : IMessageProcessor
{
}