using Serilog;

namespace CleanObjBinFolder.Prompts;

public class ApplicationPrompt
{
    public static string GetPathToRead()
    {
        string pathWrong = "-1";
        try
        {
            Console.WriteLine("Do you want to use current Path?\nPress 'y' to use current Path, Press 'n' to add the path to clear. 'y' (default)");
            string? askPathAnswer = Console.ReadLine();

            askPathAnswer = askPathAnswer!.ToLowerInvariant();

            if (askPathAnswer == "y" || string.IsNullOrWhiteSpace(askPathAnswer))
                return AppDomain.CurrentDomain.BaseDirectory;

            if (askPathAnswer == "n")
            {
                Console.WriteLine("Please add the path to clear");
                string? pathAdded = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(pathAdded))
                {
                    Console.WriteLine($"Please Add a Path");
                    return pathWrong;
                }

                return pathAdded!;
            }

            Console.WriteLine($"Cannot select {askPathAnswer} as value");
        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(GetPathToRead)} with message {ex.Message}");
            throw;
        }

        return pathWrong;
    }

    public static string ExcludeVsFolder()
    {
        string vsFolder = string.Empty;

        try
        {
            Console.WriteLine("Do you want to exclude the '.vs' folder?. 'n' (default)");
            string? vsExcludeAnswer = Console.ReadLine();
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

    public static List<string> GetExcludeFolders()
    {
        List<string> excludeFolders = new();

        try
        {
            Console.WriteLine("Do you want to exclude any folder?\nPress 'y' to exclude folders or 'n' to leave it as it is. 'n' (default)");
            string? excludeFolderAnswer = Console.ReadLine();
            excludeFolderAnswer = excludeFolderAnswer!.ToLowerInvariant();

            if (excludeFolderAnswer == "n" || string.IsNullOrWhiteSpace(excludeFolderAnswer))
                return excludeFolders;

            if (excludeFolderAnswer == "y")
            {
                Console.WriteLine("Please add the name of the folders separated by comma");
                string? excludeFolderString = Console.ReadLine();

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
                        item.Trim();
                        excludeFolders.Add(item);
                    }
                }

                return excludeFolders;
            }

            Console.WriteLine($"Cannot select {excludeFolderAnswer} as value");
            excludeFolders.Add("-1");
        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(GetExcludeFolders)} with message {ex.Message}");
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