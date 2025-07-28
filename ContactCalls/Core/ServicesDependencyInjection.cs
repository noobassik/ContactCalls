using ContactCalls.Services.Implementations;
using ContactCalls.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ContactCalls.Services;

public static class ServicesDependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IPhoneService, PhoneService>();
        services.AddScoped<ICallService, CallService>();
        services.AddScoped<IConferenceService, ConferenceService>();
        
        return services;
    }
}