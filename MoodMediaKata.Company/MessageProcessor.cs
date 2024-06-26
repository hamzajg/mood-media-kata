using System.Text.Json;
using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class MessageProcessor(CreateCompanyUseCase createNewCompanyUseCase, DeleteDevicesUseCase deleteDevicesUseCase, QueryCompanyByIdUseCase queryCompanyByIdUseCase)
{
    public async Task Process(Message message)
    {
        switch (message.MessageType)
        {
            case MessageType.NewCompany:
                var payload = message as CreateNewCompanyMessage;
                Console.WriteLine(JsonSerializer.Serialize(payload!));
                await createNewCompanyUseCase.Execute(payload!.CompanyName, payload.CompanyCode, 
                    (Licensing)Enum.Parse(typeof(Licensing), payload.Licensing), 
                    payload.Devices.Select(dto=> new Device(dto.SerialNumber, (DeviceType)Enum.Parse(typeof(DeviceType), dto.Type) )));
                break;
            case MessageType.DeleteDevices:
                Console.WriteLine(JsonSerializer.Serialize(message as DeleteDevicesMessage));
                await deleteDevicesUseCase.Execute((message as DeleteDevicesMessage)!.SerialNumbers);
                break;
            default:
                throw new NotSupportedException($"{message.MessageType} is not supported.");
        }
    }
    
    public async Task<T?> Process<T>(Message message) where T : Entity
    {
        switch (message.MessageType)
        {
            case MessageType.QueryCompanyById:
                Console.WriteLine(JsonSerializer.Serialize(message as QueryCompanyByIdMessage));
                return await queryCompanyByIdUseCase.Execute((message as QueryCompanyByIdMessage)!.CompanyId) as T;
            default:
                throw new NotSupportedException($"{message.MessageType} is not supported.");
        }
    }
}