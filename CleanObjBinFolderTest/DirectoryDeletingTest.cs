using CleanObjBinFolder;
using CleanObjBinFolder.Prompts;
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

    [Fact]
    public void RunAskDefaultPathToRead_AddWrongPathInputToRead()
    {
        var result = ApplicationPrompt.AskDefaultPathToRead("test");

        Assert.Equal("-1", result);
    }

    [Fact]
    public void RunAskDefaultPathToRead_AddYPathInputToRead()
    {
        var result = ApplicationPrompt.AskDefaultPathToRead("y");

        Assert.NotEmpty(result);
    }

    [Fact]
    public void RunAskDefaultPathToRead_AddNPathInputToRead()
    {
        var result = ApplicationPrompt.AskDefaultPathToRead("n");

        Assert.Equal("n", result);
    }

    [Fact]
    public void RunGetPathToRead_EmptyPathInputToRead()
    {
        var result = ApplicationPrompt.GetPathToRead(string.Empty);

        Assert.Equal("-1", result);
    }

    [Fact]
    public void RunGetPathToRead_RealPathInputToRead()
    {
        var result = ApplicationPrompt.GetPathToRead(DirectoryDeletingTestData.rootPath);

        Assert.Equal(DirectoryDeletingTestData.rootPath, result);
    }

    [Fact]
    public void RunExcludeVsFolder_AddWrongPathInputToRead()
    {
        var result = ApplicationPrompt.ExcludeVsFolder("test");

        Assert.Equal("-1", result);
    }

    [Fact]
    public void RunExcludeVsFolder_AddYPathInputToRead()
    {
        var result = ApplicationPrompt.ExcludeVsFolder("y");

        Assert.Equal(".vs", result);
    }

    [Fact]
    public void RunExcludeVsFolder_AddNPathInputToRead()
    {
        var result = ApplicationPrompt.ExcludeVsFolder("n");

        Assert.Empty(result);
    }

    [Fact]
    public void RunExcludeOtherFolderError_AddWrongPathInputToRead()
    {
        var result = ApplicationPrompt.GetExcludeFolderError("test");

        //Assert.Equal(1, result.Count());
        // Recomended by xunit when checking one item on collection
        Assert.Single(result);

        Assert.Collection(result, 
            item => Assert.Equal("-1", item));
    }

    [Fact]
    public void RunExcludeOlderFolderError_AddNoPathInputToRead()
    {
        var result = ApplicationPrompt.GetExcludeFolderError("n");

        Assert.Empty(result);
    }

    [Fact]
    public void RunExcludeOtherFolder_AddOneFolderToRead()
    {
        string excludeFolderString = "obj";
        var result = ApplicationPrompt.ExcludeFolderAddition(excludeFolderString);

        Assert.Single(result);

        Assert.Collection(result,
            item => Assert.Equal("obj", item));
    }

    [Fact]
    public void RunExcludeOtherFolder_AddTwoFolderToRead()
    {
        string excludeFolderString = "obj, Controller";
        var result = ApplicationPrompt.ExcludeFolderAddition(excludeFolderString);

        Assert.Equal(2, result.Count);

        Assert.Collection(result,
            item => Assert.Equal("obj", item),
            item => Assert.Equal("Controller", item));
    }
}