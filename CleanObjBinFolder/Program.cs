using CleanObjBinFolder.Extensions;
using CleanObjBinFolder.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = new ConfigurationBuilder();

ExtensionServices.BuildConfiguration(builder);

IConfiguration Configuration = builder.Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(Configuration)
    .WriteTo.Console()
    .CreateLogger();

		if (string.IsNullOrWhiteSpace(pathToSearch))
			Console.WriteLine("Need to Add a Path");
	}
	else
	{
		try
		{            
    Log.Information("Starting up the Service");

    var services = ExtensionServices.ConfigureService(Configuration);

    var serviceProvider = services.BuildServiceProvider();

    serviceProvider.GetService<ApplicationService>()!.Run();
        }
		catch (Exception ex)
		{
    Log.Fatal(ex, "There was a problem starting the service");
    return;
	}
finally
{
    Log.CloseAndFlush();
}

Console.ReadKey();