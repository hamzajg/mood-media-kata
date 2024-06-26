using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;
using MoodMediaKata.Company;
using MoodMediaKata.Database;
using MoodMediaKata.Infra;
using MoodMediaKata.Messaging;
using MoodMediaKata.Repository;

namespace MoodMediaKata;

public static class InfrastructureRegistration
{

    public static IServiceCollection InitializeDatabase(this IServiceCollection services, string[] args)
    {
        string databaseOrmType = null;
        if (args.Length > 0)
            databaseOrmType = args.FirstOrDefault(a => a.StartsWith("--db-orm="));

        if (string.IsNullOrEmpty(databaseOrmType))
            throw new ArgumentException("databaseOrmType must be provided as a runtime argument.");
        
        services.AddSingleton(DatabaseInitializerFactory.InitializeDatabase(databaseOrmType.Split("=")[1], services));
        
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, string[] args)
    {
        string repositoryType = null;
        if (args.Length > 0)
            repositoryType = args.FirstOrDefault(a => a.StartsWith("--repository="));

        if (string.IsNullOrEmpty(repositoryType))
            throw new ArgumentException("repository must be provided as a runtime argument.");
        
        services.AddSingleton<IRepository<Company.Company>>(provider => RepositoryFactory.CreateRepository<Company.Company>(repositoryType.Split("=")[1], provider));
        services.AddSingleton<IRepository<Location>>(provider => RepositoryFactory.CreateRepository<Location>(repositoryType.Split("=")[1], provider));
        services.AddSingleton<IDeviceRepository>(provider => RepositoryFactory.CreateRepository<IDeviceRepository, Device>(repositoryType.Split("=")[1], provider));
        
        return services;
    }
    
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, string[] args)
    {
        services.AddSingleton(RabbitHutch.CreateBus("host=127.0.0.1:5672;username=guest;password=guest"));
        services.AddSingleton<MessageProcessor>(); 
        services.AddSingleton<MessageDispatcher>(); 
        services.AddScoped<CreateNewCompanyMessageHandler>();
        services.AddScoped<DeleteDevicesMessageHandler>();
        services.AddScoped<QueryCompanyByIdMessageHandler>();
        
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
        provider.GetService<IBus>()?.PubSub.Subscribe<DeleteDevicesMessage>(queueName, 
            msg => provider.GetService<DeleteDevicesMessageHandler>()?.ConsumeAsync(msg));
        provider.GetService<IBus>()?.PubSub.Subscribe<QueryCompanyByIdMessage>(queueName, 
            msg => provider.GetService<QueryCompanyByIdMessageHandler>()?.ConsumeAsync(msg));
        return provider;
    }
    
}
