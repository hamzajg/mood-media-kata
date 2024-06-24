using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;

namespace MoodMediaKata.Infra;

public static class InfrastructureRegistration
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IRepository<Entity>>(_ =>
        {
            var repositoryType = ConfigurationManager.AppSettings["RepositoryType"];
            return RepositoryFactory.CreateRepository(repositoryType);
        });

        services.AddSingleton<IMessageProcessor>(_ =>
        {
            var messageProcessorType = ConfigurationManager.AppSettings["MessageProcessorType"];
            return MessageProcessorFactory.CreateMessageProcessor(messageProcessorType);
        });
        return services;
    }
}