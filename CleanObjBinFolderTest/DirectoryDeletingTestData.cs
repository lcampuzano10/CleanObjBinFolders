using System.IO.Abstractions.TestingHelpers;

namespace CleanObjBinFolderTest;

public class DirectoryDeletingTestData
{
    private readonly MockFileSystem _fileSystem;
    public static string rootPath = "TimeSheetSln";
    public static string firstChild = Path.Combine(rootPath, "TimeSheetAPI");
    public static string vsFolder = Path.Combine(rootPath, ".vs");
    public static string objFolder = Path.Combine(firstChild, "obj");
    public static string binFolder = Path.Combine(firstChild, "bin");
    public static string controllersFolder = Path.Combine(firstChild, "Controllers");
    public static string propertiesFolder = Path.Combine(firstChild, "Properties");

    public DirectoryDeletingTestData(MockFileSystem fileSystem)
    {
        _fileSystem = fileSystem;

        _fileSystem.AddDirectory(firstChild);
        _fileSystem.AddDirectory(vsFolder);
        _fileSystem.AddDirectory(objFolder);
        _fileSystem.AddDirectory(binFolder);
        _fileSystem.AddDirectory(controllersFolder);
        _fileSystem.AddDirectory(propertiesFolder);
    }
}