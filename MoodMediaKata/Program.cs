// See https://aka.ms/new-console-template for more information

using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;
using MoodMediaKata.Infra;

Console.WriteLine("Hello, World!");
var services = new ServiceCollection();
services.RegisterInfrastructure();
var serviceProvider = services.BuildServiceProvider();
var repository = serviceProvider.GetService<IRepository<Entity>>();