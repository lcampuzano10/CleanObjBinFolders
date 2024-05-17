using CleanObjBinFolder;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.IO.Abstractions.TestingHelpers;

namespace CleanObjBinFolderTest;

public class DirectoryDeletingTest
{
    private readonly MockFileSystem _fileSystem;
    private readonly DirectoryDeleting _deleting;
    private readonly ILogger<DirectoryDeleting> _logger;

    public DirectoryDeletingTest()
    {
        _logger = Substitute.For<ILogger<DirectoryDeleting>>();
        _fileSystem = new MockFileSystem();
        _deleting = new DirectoryDeleting(_logger, _fileSystem);
        DirectoryDeletingTestData testData = new DirectoryDeletingTestData(_fileSystem);
    }

    [Fact]
    public void GivenAPath_WhenCheckingExcludeDirectory_ReturnCountOfPath()
    {
        List<string> folderExclude = new()
            {
                "Properties", "Angular", "Timesheets"
            };

        _deleting.FindDirectoryToDelete(DirectoryDeletingTestData.firstChild, folderExclude);

        Assert.Equal(2, _deleting.PathsToDelete.Count);
    }

    [Fact]
    public void GivenAPath_WhenCheckingExcludeBinDirectory_ReturnSinglePath()
    {
        List<string> folderExclude = new()
            {
                "Properties", "Angular", "Timesheets", "bin"
            };

        _deleting.FindDirectoryToDelete(DirectoryDeletingTestData.firstChild, folderExclude);

        Assert.Single(_deleting.PathsToDelete);
    }

    [Fact]
    public void GivenAPath_WhenCheckingVsDirectory_ReturnCountOfPath()
    {
        List<string> folderExclude = new()
            {
                "Properties", "Angular", "Timesheets", ".vs"
            };

        _deleting.FindDirectoryToDelete(DirectoryDeletingTestData.rootPath, folderExclude);

        Assert.Equal(2, _deleting.PathsToDelete.Count);
    }

    [Fact]
    public void GivenAPath_WhenCheckingNoExcludeDirectory_ReturnCountOfPath()
    {
        List<string> folderExclude = new();

        _deleting.FindDirectoryToDelete(DirectoryDeletingTestData.rootPath, folderExclude);

        Assert.Equal(3, _deleting.PathsToDelete.Count);
    }

    [Fact]
    public void RunMethodToDelete_AddDeletePaths_ReturnCountOfPath()
    {
        _deleting.PathsToDelete.Add(DirectoryDeletingTestData.binFolder);
        _deleting.PathsToDelete.Add(DirectoryDeletingTestData.objFolder);

        _deleting.DeleteFolders();

        Assert.Equal(4, _fileSystem.AllNodes.Count());
    }
}