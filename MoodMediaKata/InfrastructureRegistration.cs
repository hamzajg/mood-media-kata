using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;
using MoodMediaKata.Company;
using MoodMediaKata.Infra;

namespace MoodMediaKata;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, string[] args)
    {
        string repositoryType = null;
        if (args.Length > 0)
            repositoryType = args.FirstOrDefault(a => a.StartsWith("--repository="));

        if (string.IsNullOrEmpty(repositoryType))
            throw new ArgumentException("repository must be provided as a runtime argument.");
        
        services.AddSingleton<IRepository<Company.Company>>(_ => RepositoryFactory.CreateRepository<Company.Company>(repositoryType.Split("=")[1]));
        services.AddSingleton<IRepository<Location>>(_ => RepositoryFactory.CreateRepository<Location>(repositoryType.Split("=")[1]));
        services.AddSingleton<IDeviceRepository, InMemoryDeviceRepository>();
        return services;
    }
    
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, string[] args)
    {
        services.AddSingleton(RabbitHutch.CreateBus("host=127.0.0.1:5672;username=guest;password=guest"));
        services.AddSingleton<MessageProcessor>(); 
        services.AddSingleton<MessageDispatcher>(); 

        /*services.AddSingleton(s => new AutoSubscriber(s.GetService<IBus>(), "subscription_id")
        {
            AutoSubscriberMessageDispatcher = new DefaultAutoSubscriberMessageDispatcher(s.GetService<IServiceResolver>())
        });*/
        services.AddScoped<CreateNewCompanyMessageHandler>();
        
        string messageProcessorType = null;
        if (args.Length > 0)
            messageProcessorType = args.FirstOrDefault(a => a.StartsWith("--message-processor="));;

        if (string.IsNullOrEmpty(messageProcessorType))
            throw new ArgumentException("MessageProcessorType must be provided as a runtime argument.");
        services.AddSingleton<IMessageProcessor>(_ => MessageProcessorFactory.CreateMessageProcessor(messageProcessorType.Split("=")[1]));
        return services;
    }
    
    public static IServiceProvider AddMessageHandler(this IServiceProvider provider)
    {
        const string queueName = "Q.MoodMediaKata";
        provider.GetService<IBus>()?.PubSub.Subscribe<CreateNewCompanyMessage>(queueName, 
            msg => provider.GetService<CreateNewCompanyMessageHandler>()?.ConsumeAsync(msg));
        return provider;
    }
    
}