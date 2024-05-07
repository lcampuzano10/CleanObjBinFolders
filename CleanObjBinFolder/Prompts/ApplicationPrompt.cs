using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                    Console.WriteLine("Please add comma between the names");
                    excludeFolders.Add("-1");
                    return excludeFolders;
                }

                string[] asArray = [.. excludeFolderString.Split(',')];

                foreach (var item in asArray)
                    item.Trim();

                return asArray.ToList();
            }

            Console.WriteLine($"Cannot select {excludeFolderAnswer} as value");
            excludeFolders.Add("-1");

        }
        catch (Exception ex)
        {
            Log.Error($"Error at {nameof(GetExcludeFolders)} with message {ex.Message}");
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
