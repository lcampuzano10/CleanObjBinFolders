using CleanObjBinFolder.Constants;
using CleanObjBinFolder.Extensions;
using Serilog;
using System.IO;

namespace CleanObjBinFolder
{
    public class DirectoryDeleting
    {
        public List<string> PathsToDelete { get; set; } = new();
        public long SumFileDelete = 0;

        public void FindDirectoryToDelete(string parentPath, List<string> excludeFolders)
        {
            List<string> findParents = new();

            try
            {
                excludeFolders.Add(DirectoryConstants.GitFolder);

                if (Directory.Exists(parentPath))
                {
                    if (excludeFolders.Any())
                        findParents = Directory.GetDirectories(parentPath)
                            .Where(d => !excludeFolders.Any(d.Contains)).ToList();  // <= Will exclude the folders and show only the desire one to delete.
                    else
                        findParents = Directory.GetDirectories(parentPath).ToList();    // <= Will find all and delete .vs, bin, obj

                    var containsCS = findParents.Where(_ => _.Contains(DirectoryConstants.VSFolder));

                    if (containsCS.Any())
                        PathsToDelete.Add(containsCS.First());

                    var othersFolders = findParents.Where(_ => !_.Contains(DirectoryConstants.VSFolder));

                    var containsObjBin = findParents.Where(_ => _.Contains(DirectoryConstants.BinFolder) || _.Contains(DirectoryConstants.ObjFolder));

                    if (containsObjBin.Any())
                        foreach (var path in containsObjBin)
                            PathsToDelete.Add(path);
                    else
                        foreach (var pathParent in othersFolders)
                            FindDirectoryToDelete(pathParent, excludeFolders);
                }

            }
            catch (Exception ex)
            {
                Log.Error($"Error at {nameof(FindDirectoryToDelete)} with message {ex.Message}");
                throw;
            }
        }

        public bool DeleteDirectoryFromPath(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    SumFileDelete += DirectoryExtension.GetFolderSizeNoExtension(path, true);
                    Directory.Delete(path, true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error at {nameof(DeleteDirectoryFromPath)} with message {ex.Message}");
                throw;
            }

            return false;
        }

        public static void DeleteDirectory(string path)
        {
            //var directoryInfo = new DirectoryInfo(path);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);

                Console.WriteLine($"===========================================");
                Console.WriteLine($"Path: {path} Found and Deleted");
                Console.WriteLine($"===========================================");
            }
        }
    }
}