// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata;
using MoodMediaKata.Company;

Console.WriteLine("Hello, World!");
var services = new ServiceCollection();
services.AddRepositories(args);
services.AddUseCases();
services.RegisterInfrastructure(args);
var serviceProvider = services.BuildServiceProvider();
//await serviceProvider.GetRequiredService<AutoSubscriber>().SubscribeAsync(new[] { typeof(CreateNewCompanyMessageHandler).Assembly });
serviceProvider.AddMessageHandler();
Console.ReadLine();