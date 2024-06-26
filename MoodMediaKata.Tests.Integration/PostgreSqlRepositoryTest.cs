using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Integration;

[Collection("SequentialTests")]
public class PostgreSqlRepositoryTest
{
    private readonly ServiceProvider _provider;

    public PostgreSqlRepositoryTest()
    {
        IdGenerator.StartFrom(100 + new Random().Next());
        
        var services = new ServiceCollection();
        services.InitializeDatabase(new []{"--db-orm=dapper"});
        _provider = services.BuildServiceProvider();
        _provider.GetService<IDatabaseInitializer>()?.InitializeDatabase();
    }
    
    [Fact]
    public async void CanSaveCompanyEntity()
    {
        var repository =
            RepositoryFactory.CreateRepository<Company.Company>("PostgreSql",
                _provider);
        
        var result = await repository.Save(new Company.Company("New Company 1", $"COMP-123-{new Random().Next()}", 
            Licensing.Standard, Array.Empty<Device>()));
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async void CanSaveLocationEntity()
    {
        var repository = RepositoryFactory.CreateRepository<Company.Company>("PostgreSql", _provider);
        var locationRepository = RepositoryFactory.CreateRepository<Location>("PostgreSql", _provider);
        var company = new Company.Company("New Company 1", $"COMP-123-{new Random().Next()}",
            Licensing.Standard, new[] { new Device("SerialNumber1", DeviceType.Standard) });
        await repository.Save(company);

        var result = await locationRepository.Save(company.Locations.ElementAt(0));
            
        Assert.NotNull(result);
    }
    
    [Fact]
    public async void CanSaveDeviceEntity()
    {
        var repository = RepositoryFactory.CreateRepository<Company.Company>("PostgreSql", _provider);
        var locationRepository = RepositoryFactory.CreateRepository<Location>("PostgreSql", _provider);
        var deviceRepository = RepositoryFactory.CreateRepository<IDeviceRepository, Device>("PostgreSql", _provider);
        var company = new Company.Company("New Company 1", $"COMP-123-{new Random().Next()}",
            Licensing.Standard, new[] { new Device("SerialNumber1", DeviceType.Standard) });
        await repository.Save(company);
        await locationRepository.Save(company.Locations.ElementAt(0));

        var result = deviceRepository.Save(company.Locations.ElementAt(0).Device);
            
        Assert.NotNull(result);
    }
}