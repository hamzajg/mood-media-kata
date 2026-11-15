using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Messaging;

public class AddDevicesMessageHandler(MessageProcessor messageProcessor) : IConsumeAsync<AddDevicesMessage>
{
    public async Task ConsumeAsync(AddDevicesMessage message,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await messageProcessor.Process(message);
    }
}

public class QueryLocationByIdMessageHandler(IBus bus, MessageProcessor messageProcessor)
    : IConsumeAsync<QueryLocationByIdMessage>
{
    public async Task ConsumeAsync(QueryLocationByIdMessage message,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await messageProcessor.Process<Location>(message);

        await bus.PubSub.PublishAsync(
            new QueryLocationByIdResultMessage
            {
                RequestId = message.Id,
                Location = result is null
                    ? null
                    : new LocationDto
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Address = result.Address,
                        CompanyId = result.Company?.Id ?? 0,
                        DeviceSerialNumber = result.Device?.SerialNumber,
                        DeviceType = result.Device?.Type
                    }
            }, QueryLocationByIdResultMessage.Queue);
    }
}
