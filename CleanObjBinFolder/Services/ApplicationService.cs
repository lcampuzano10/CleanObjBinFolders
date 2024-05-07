using CleanObjBinFolder.Extensions;
using CleanObjBinFolder.Models;
using CleanObjBinFolder.Prompts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace CleanObjBinFolder.Services;

public interface IApplicationService
{
    void Run();
}

public class ApplicationService(IConfiguration configuration, ILogger<ApplicationService> logger)
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly ILogger<ApplicationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public void Run()
    {
        //var applicationInfo = _configuration.GetSection("ApplicationInfo")!.Get<ApplicationInfo>()!;

        try
        {
            string pathToRead = ApplicationPrompt.GetPathToRead();

            if (pathToRead == "-1")
            {
                RunErrorMessage();
            }

            List<string> excudeFolders = ApplicationPrompt.GetExcludeFolders();

            if (excudeFolders.Count > 0 && excudeFolders[0] == "-1")
            {
                RunErrorMessage();
            }

            DirectoryDeleting directoryDeleting = new DirectoryDeleting();
            directoryDeleting.FindDirectoryToDelete(pathToRead, excudeFolders);

            foreach (var pathToDelete in directoryDeleting.PathsToDelete)
            {
                if (directoryDeleting.DeleteDirectoryFromPath(pathToDelete))
                {
                    string successMessage = $"Directory {pathToDelete} has been deleted successfully";
                    Console.WriteLine(successMessage);
                    _logger.LogInformation(successMessage);
                }
                else
                {
                    string warningMessage = $"Directory {pathToDelete} couldn't be found and/or deleted";
                    Console.WriteLine(warningMessage);
                    _logger.LogWarning(warningMessage);
                }
            }

            ApplicationPrompt.DeletedFolderMessage(directoryDeleting.PathsToDelete.Count, DirectoryExtension.FormatFileSize(directoryDeleting.SumFileDelete));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    private void RunErrorMessage()
    {
        Console.WriteLine($"Running application again");
        _logger.LogInformation("Running application again");
        Thread.Sleep(new TimeSpan(0, 0, 5));
        Console.Clear();
        Run();
    }
}