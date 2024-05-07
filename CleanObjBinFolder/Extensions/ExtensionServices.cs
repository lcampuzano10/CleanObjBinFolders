using CleanObjBinFolder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CleanObjBinFolder.Extensions;

public static class ExtensionServices
{
    public static void BuildConfiguration(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}\\appsettings.json",
                optional: false, reloadOnChange: true)
            .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}\\appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    public static IServiceCollection ConfigureService(IConfiguration configuration)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!.Equals("Development"))
            {
                builder.SetMinimumLevel(LogLevel.Debug);
            }
            else
            {
                builder.SetMinimumLevel(LogLevel.Warning);
            }
            builder.AddSerilog(Log.Logger, true);
        });
        services.AddSingleton(configuration); 
        services.AddOptions();
        services.AddTransient<ApplicationService>();

        return services;
    }
}