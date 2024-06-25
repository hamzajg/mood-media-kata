// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata;
using MoodMediaKata.Company;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();
services.InitializeDatabase(args);
services.AddRepositories(args);
services.AddUseCases();
services.RegisterInfrastructure(args);

var serviceProvider = services.BuildServiceProvider();

serviceProvider.AddMessageHandler();
serviceProvider.GetService<IDatabaseInitializer>()?.InitializeDatabase();

Console.ReadLine();