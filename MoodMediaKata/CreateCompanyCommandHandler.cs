using EasyNetQ.AutoSubscribe;

namespace MoodMediaKata;

public class CreateCompanyCommand
{
    public string Name { get; set; }
}
public class CreateCompanyCommandHandler : IConsumeAsync<CreateCompanyCommand>
{
    public Task ConsumeAsync(CreateCompanyCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        Console.WriteLine($"Received message: {message.Name}");
        return Task.CompletedTask;
    }
}