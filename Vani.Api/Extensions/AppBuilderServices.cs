using Vani.Infras;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vani.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGarageDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<VaniDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("default");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

            options.UseMySql(connectionString, serverVersion);
        });

        return services;
    }

    public static IServiceCollection AddAppDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("default");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));

            options.UseMySql(connectionString, serverVersion);
        });

        return services;
    }
}
