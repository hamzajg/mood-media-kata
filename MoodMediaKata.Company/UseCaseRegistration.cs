using Microsoft.Extensions.DependencyInjection;

namespace MoodMediaKata.Company;

public static class UseCaseRegistration
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<CreateCompanyUseCase>();
        services.AddScoped<AddDevicesUseCase>();
        services.AddScoped<DeleteDevicesUseCase>();
        services.AddScoped<QueryCompanyByIdUseCase>();
        return services;
    }
}