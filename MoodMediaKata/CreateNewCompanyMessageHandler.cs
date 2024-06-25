using EasyNetQ.AutoSubscribe;
using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata;

public class CreateNewCompanyMessageHandler(MessageProcessor messageProcessor) : IConsumeAsync<CreateNewCompanyMessage>
{
    public Task ConsumeAsync(CreateNewCompanyMessage message, CancellationToken cancellationToken = new CancellationToken())
    {
        messageProcessor.Process(message);
        return Task.CompletedTask;
    }
}