using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class DeleteDevicesUseCaseTest : IDisposable
{    
    private readonly IDeviceRepository _deviceRepository = new InMemoryDeviceRepository();

    private readonly DeleteDevicesUseCase _sut;

    public DeleteDevicesUseCaseTest()
    {
        _sut = new DeleteDevicesUseCase(_deviceRepository);
    }

    [Fact]
    public void CanDeleteDevicesByGivenSerialNumbers()
    {
        _deviceRepository.Save(new Device("Serial1", DeviceType.Standard));
        _deviceRepository.Save(new Device("Serial2", DeviceType.Custom));

        _sut.Execute(new [] { "Serial1", "Serial2" });
        
        Assert.Null(_deviceRepository.FindOneById(1));
        Assert.Null(_deviceRepository.FindOneById(2));
    }
    
    public void Dispose()
    {
        IdGenerator.Reset();
    }
}