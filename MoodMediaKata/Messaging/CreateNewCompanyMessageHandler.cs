using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Messaging;

public class CreateNewCompanyMessageHandler(MessageProcessor messageProcessor) : IConsumeAsync<CreateNewCompanyMessage>
{
    public async Task ConsumeAsync(CreateNewCompanyMessage message,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await messageProcessor.Process(message);
    }
}

public class DeleteDevicesMessageHandler(MessageProcessor messageProcessor) : IConsumeAsync<DeleteDevicesMessage>
{
    public async Task ConsumeAsync(DeleteDevicesMessage message,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await messageProcessor.Process(message);
    }
}

public class QueryCompanyByIdMessageHandler(IBus bus, MessageProcessor messageProcessor)
    : IConsumeAsync<QueryCompanyByIdMessage>
{
    public async Task ConsumeAsync(QueryCompanyByIdMessage message,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await messageProcessor.Process<Company.Company>(message);

        const string queueName = "Q.MoodMediaKata";
        await bus.PubSub.PublishAsync(
            new QueryCompanyByIdResultMessage
                { Company = new CompanyDto { Id = result.Id, Name = result.Name, Code = result.Code , Licensing = result.Licensing } }, queueName);
    }
}