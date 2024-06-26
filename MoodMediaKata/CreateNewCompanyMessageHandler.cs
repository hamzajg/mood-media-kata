using EasyNetQ.AutoSubscribe;
using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata;

public class CreateNewCompanyMessageHandler(MessageProcessor messageProcessor) : IConsumeAsync<CreateNewCompanyMessage>
{
    public async Task ConsumeAsync(CreateNewCompanyMessage message, CancellationToken cancellationToken = new CancellationToken())
    {
        await messageProcessor.Process(message);
    }
}

public class DeleteDevicesMessageHandler(MessageProcessor messageProcessor) : IConsumeAsync<DeleteDevicesMessage>
{
    public async Task ConsumeAsync(DeleteDevicesMessage message, CancellationToken cancellationToken = new CancellationToken())
    {
        await messageProcessor.Process(message);
    }
}
