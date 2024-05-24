using Serilog;

namespace CleanObjBinFolder.Prompts;

public class ApplicationPrompt
{
    public static string AskDefaultPathToRead(string askPathAnswer)
    {
        string pathWrong = "-1";
        try
        {
            askPathAnswer = askPathAnswer!.ToLowerInvariant();

            if (askPathAnswer == "y" || string.IsNullOrWhiteSpace(askPathAnswer))
                return AppDomain.CurrentDomain.BaseDirectory;

            if (askPathAnswer == "n")
                pathWrong = askPathAnswer;

            Console.WriteLine($"Cannot select {askPathAnswer} as value");
        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(AskDefaultPathToRead)} with message {ex.Message}");
            throw;
        }

        return pathWrong;
    }

    public static string GetPathToRead(string pathAdded)
    {
        string pathWrong = "-1";

        try
        {
            if (string.IsNullOrWhiteSpace(pathAdded))
            {
                Console.WriteLine($"Please Add a Path");
                return pathWrong;
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(GetPathToRead)} with message {ex.Message}");
            throw;
        }

        return pathAdded!;
    }

    public static string ExcludeVsFolder(string? vsExcludeAnswer)
    {
        string vsFolder = string.Empty;

        try
        {            
            vsExcludeAnswer = vsExcludeAnswer!.ToLowerInvariant();

            if (vsExcludeAnswer == "n" || string.IsNullOrWhiteSpace(vsExcludeAnswer))
                return vsFolder;

            if (vsExcludeAnswer == "y")
                return vsFolder = ".vs";

            Console.WriteLine($"Cannot select {vsExcludeAnswer} as value");
            vsFolder = "-1";
        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(ExcludeVsFolder)} with message {ex.Message}");
        }

        return vsFolder;
    }

    public static List<string> GetExcludeFolderError(string? excludeFolderAnswer)
    {
        List<string> excludeFolders = new();

        try
        {
            excludeFolderAnswer = excludeFolderAnswer!.ToLowerInvariant();

            if (excludeFolderAnswer == "n" || string.IsNullOrWhiteSpace(excludeFolderAnswer))
                return excludeFolders;

            Console.WriteLine($"Cannot select {excludeFolderAnswer} as value");
            excludeFolders.Add("-1");
        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(GetExcludeFolderError)} with message {ex.Message}");
            throw;
        }

        return excludeFolders;
    }

    public static List<string> ExcludeFolderAddition(string? excludeFolderString)
    {
        List<string> excludeFolders = new();

        try
        {
            if (!excludeFolderString!.Contains(','))
            {
                excludeFolders.Add(excludeFolderString);
                return excludeFolders;
            }

            string[] asArray = [.. excludeFolderString.Split(',')];

            foreach (var item in asArray)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    string itemCleaned = item.Trim();
                    excludeFolders.Add(itemCleaned);
                }
            }

            return excludeFolders;
        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(ExcludeFolderAddition)} with message {ex.Message}");
            throw;
        }

        return excludeFolders;
    }

    public static void DeletedFolderMessage(int count, string sizeDeleted)
    {
        string deletedMessage = $"Deleted {count} folders.\nDeleted {sizeDeleted} folders.";
        Console.WriteLine();
        Console.WriteLine(deletedMessage);
        Console.WriteLine("Finished. Press any key to close this window.");

        Log.Information(deletedMessage);
    }
}