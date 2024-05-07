namespace CleanObjBinFolder.Extensions;

public static class DirectoryExtension
{
    public static long GetFolderSize(string path, bool allDirectories, string extension)
    {
        var option = allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        return new DirectoryInfo(path).EnumerateFiles("*" + extension, option).Sum(file => file.Length);
    }

    public static long GetFolderSizeNoExtension(string path, bool allDirectories)
    {
        var option = allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        return new DirectoryInfo(path).EnumerateFiles("*", option).Sum(file => file.Length);
    }

    public static string FormatFileSize(long bytes)
    {
        var bUnit = 1024;
        if (bytes < bUnit)
            return $"{bytes} B";

        int exponential = (int)(Math.Log(bytes) / Math.Log(bUnit));
        return $"{bytes / Math.Pow(bUnit, exponential):F2} {("KMGTPE")[exponential - 1]}B";
    }
}