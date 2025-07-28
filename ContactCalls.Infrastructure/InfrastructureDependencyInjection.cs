using ContactCalls.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ContactCalls.Infrastructure;

public static class InfrastructureDependencyInjection 
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();

        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(sp.GetRequiredService<IOptions<DbSettings>>().Value.ConnectionString));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        return services;
    }
}