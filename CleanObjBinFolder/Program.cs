
using CleanObjBinFolder;

string pathToSearch = string.Empty;
//pathToSearch = AppDomain.CurrentDomain.BaseDirectory;
pathToSearch = Console.ReadLine();
long sumFileDelete = 0;
DirectoryDeleting directoryDeleting = new DirectoryDeleting();
var listPathToDelete = directoryDeleting.PathsToDelete;

Console.WriteLine($"Path to look: {pathToSearch}");

if (string.IsNullOrWhiteSpace(pathToSearch))
{
	Console.WriteLine("Need to Add a Path");
}
else
{
	if (pathToSearch.Contains("\""))
	{
		pathToSearch = pathToSearch.Replace("\"", "");

		if (string.IsNullOrWhiteSpace(pathToSearch))
			Console.WriteLine("Need to Add a Path");
	}
	else
	{
		try
		{            
			directoryDeleting.RecursiveDirectory(pathToSearch);

			foreach (var pathToDelete in listPathToDelete)
			{
				sumFileDelete += directoryDeleting.GetFolderSizeNoExtension(pathToDelete, true);
                DirectoryDeleting.DeleteDirectory(pathToDelete);
            }

			directoryDeleting.DeletedFolderMessage(listPathToDelete.Count, DirectoryDeleting.FormatFileSize(sumFileDelete));

        }
		catch (Exception ex)
		{
			Console.WriteLine("There was an issue:");
			Console.WriteLine(ex.Message);
            directoryDeleting.DeletedFolderMessage(listPathToDelete.Count, DirectoryDeleting.FormatFileSize(sumFileDelete));
        }
	}
}

Console.ReadKey();