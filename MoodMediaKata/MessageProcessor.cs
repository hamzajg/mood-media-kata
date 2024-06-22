namespace MoodMediaKata;

public class MessageProcessor(CreateCompanyUseCase createNewCompanyUseCase)
{
    public void Process(Message message)
    {
        switch (message.MessageType)
        {
            case MessageType.NewCompany:
                var payload = message as NewCompanyMessage;
                createNewCompanyUseCase.Execute(payload!.CompanyName, payload.CompanyCode, 
                    (Licensing)Enum.Parse(typeof(Licensing), payload.Licensing), 
                    payload.Devices.Select(dto=> new Device(dto.SerialNumber, (DeviceType)Enum.Parse(typeof(DeviceType), dto.Type) )));
                break;
            default:
                throw new NotSupportedException($"{message.MessageType} is not supported.");
        }
    }
}