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
            List<string> excludeFoldersList = new();

            Console.WriteLine("Do you want to use current Path?\nPress 'y' to use current Path, Press 'n' to add the path to clear. 'y' (default)");
            string askPathAnswer = Console.ReadLine()!;

            string pathToRead = ApplicationPrompt.AskDefaultPathToRead(askPathAnswer);

            if(pathToRead == "n")
            {
                Console.WriteLine("Please add the path to clear");
                string pathAdded = Console.ReadLine()!;

                pathToRead = ApplicationPrompt.GetPathToRead(pathAdded!);
            }

            if (pathToRead == "-1")
                RunErrorMessage();

            Console.WriteLine("Do you want to exclude the '.vs' folder?. 'n' (default)");
            string? vsExcludeAnswer = Console.ReadLine();

            string excludingVS = ApplicationPrompt.ExcludeVsFolder(vsExcludeAnswer!);

            if (excludingVS == "-1")
                RunErrorMessage();

            if (!string.IsNullOrWhiteSpace(excludingVS))
                excudeFolders.Add(excludingVS);

            Console.WriteLine("Do you want to exclude any folder?\nPress 'y' to exclude folders or 'n' to leave it as it is. 'n' (default)");
            string? excludeFolderAnswer = Console.ReadLine();

            if(excludeFolderAnswer == "y")
            {
                Console.WriteLine("Please add the name of the folders separated by comma");
                string? excludeFolderString = Console.ReadLine();

                excludeFoldersList = ApplicationPrompt.ExcludeFolderAddition(excludeFolderString!);
            }
            else
            {
                excludeFoldersList = ApplicationPrompt.GetExcludeFolderError(excludeFolderAnswer);

                if (excludeFoldersList.Count > 0 && excludeFoldersList.Any(_ => _.Equals("-1")))
                    RunErrorMessage();
            }

            excudeFolders.AddRange(excludeFoldersList);

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
        _logger.LogInformation("Running application again");
        Thread.Sleep(new TimeSpan(0, 0, 5));
        Console.Clear();
        Run();
    }
}