using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;
using MoodMediaKata.Infra;

namespace MoodMediaKata;

public static class InfrastructureRegistration
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        var bus = RabbitHutch.CreateBus("host=127.0.0.1:5672;username=guest;password=guest");
        services.AddSingleton(bus);

        const string queueName = "Q.MoodMediaKata";

        services.AddSingleton<MessageDispatcher>(); 
        bus.PubSub.Subscribe<CreateCompanyCommand>(queueName, msg => new CreateCompanyCommandHandler().ConsumeAsync(msg));

        services.AddSingleton(s => new AutoSubscriber(s.GetService<IBus>(), "subscription_id")
        {
            AutoSubscriberMessageDispatcher = new DefaultAutoSubscriberMessageDispatcher(s.GetService<IServiceResolver>())
        });
        // message handlers
        services.AddScoped<CreateCompanyCommandHandler>();
        /*services.AddSingleton<IRepository<Entity>>(_ =>
        {
            var repositoryType = ConfigurationManager.AppSettings["RepositoryType"];
            return RepositoryFactory.CreateRepository(repositoryType);
        });*/

        /*services.AddSingleton<IMessageProcessor>(_ =>
        {
            var messageProcessorType = ConfigurationManager.AppSettings["MessageProcessorType"];
            return MessageProcessorFactory.CreateMessageProcessor(messageProcessorType);
        });*/
        return services;
    }
}