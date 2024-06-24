// See https://aka.ms/new-console-template for more information

using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata;
using MoodMediaKata.Infra;

Console.WriteLine("Hello, World!");
var services = new ServiceCollection();
services.RegisterInfrastructure();
var serviceProvider = services.BuildServiceProvider();
await serviceProvider.GetRequiredService<AutoSubscriber>().SubscribeAsync(new[] { typeof(CreateCompanyCommandHandler).Assembly });

Console.ReadLine();