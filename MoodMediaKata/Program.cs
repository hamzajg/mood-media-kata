// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata;
using MoodMediaKata.Company;
using MoodMediaKata.Database;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();
services.InitializeDatabase(args)
    .AddRepositories(args)
    .AddUseCases()
    .RegisterInfrastructure(args);

var serviceProvider = services.BuildServiceProvider()
    .AddMessageHandler();
serviceProvider.GetService<IDatabaseInitializer>()?.InitializeDatabase();

Console.ReadLine();