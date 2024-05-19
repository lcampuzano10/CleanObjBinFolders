using CleanObjBinFolder.Constants;
using CleanObjBinFolder.Extensions;
using CleanObjBinFolder.Prompts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanObjBinFolder.Services;

public interface IApplicationService
{
    void Run();
}

public class ApplicationService(ILogger<ApplicationService> logger, DirectoryDeleting directoryDeleting)
{
    private readonly ILogger<ApplicationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly DirectoryDeleting _directoryDeleting = directoryDeleting ?? throw new ArgumentNullException(nameof(directoryDeleting));

    public void Run()
    {
        try
        {
            List<string> excudeFolders = new();

            string pathToRead = ApplicationPrompt.GetPathToRead();

            if (pathToRead == "-1")
                RunErrorMessage();

            var excludingVS = ApplicationPrompt.ExcludeVsFolder();

            if (excludingVS == "-1")
                RunErrorMessage();

            if (!string.IsNullOrWhiteSpace(excludingVS))
                excudeFolders.Add(excludingVS);

            excudeFolders.AddRange(ApplicationPrompt.GetExcludeFolders());

            if (excudeFolders.Count > 0 && excudeFolders.Any(_ => _.Equals("-1")))
                RunErrorMessage();

            excudeFolders.Add(DirectoryConstants.GitFolder);

            _directoryDeleting.FindDirectoryToDelete(pathToRead, excudeFolders);

            _directoryDeleting.DeleteFolders();

            ApplicationPrompt.DeletedFolderMessage(_directoryDeleting.PathsToDelete.Count, DirectoryExtension.FormatFileSize(_directoryDeleting.SumFileDelete));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    private void RunErrorMessage()
    {
        //Console.WriteLine($"Running application again");
        _logger.LogInformation("Running application again");
        Thread.Sleep(new TimeSpan(0, 0, 5));
        Console.Clear();
        Run();
    }
}