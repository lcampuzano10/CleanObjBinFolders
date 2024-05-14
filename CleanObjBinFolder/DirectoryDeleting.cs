﻿using CleanObjBinFolder.Constants;
using CleanObjBinFolder.Extensions;
using Serilog;
using System.IO.Abstractions;

namespace CleanObjBinFolder
{
    public class DirectoryDeleting
    {
        private IFileSystem _fileSystem;
        public List<string> PathsToDelete { get; set; } = new();
        public long SumFileDelete = 0;

        public DirectoryDeleting(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void FindDirectoryToDelete(string parentPath, List<string> excludeFolders)
        {
            List<string> findParents = new();

            string pathVsCombined = Path.Combine(parentPath, DirectoryConstants.VSFolder);
            string pathBinCombined = Path.Combine(parentPath, DirectoryConstants.BinFolder);
            string pathObjCombined = Path.Combine(parentPath, DirectoryConstants.ObjFolder);

            try
            {
                excludeFolders.Add(DirectoryConstants.GitFolder);

                if (_fileSystem.Directory.Exists(parentPath))
                {
                    if (excludeFolders.Any())
                        findParents = _fileSystem.Directory.GetDirectories(parentPath)
                            .Where(d => !excludeFolders.Any(d.Contains)).ToList();  // <= Will exclude the folders and show only the desire one to delete.
                    else
                        findParents = _fileSystem.Directory.GetDirectories(parentPath).ToList();    // <= Will find all and delete .vs, bin, obj

                    var containVS = findParents.Where(_ => _.Equals(pathVsCombined));

                    if (containVS.Any())
                        PathsToDelete.Add(containVS.First());

                    var othersFolders = findParents.Where(_ => !_.Equals(pathVsCombined));

                    var containsObjBin = findParents.Where(_ => _.Equals(pathBinCombined) || _.Equals(pathObjCombined));

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
            }
        }

        public bool DeleteDirectoryFromPath(string path)
        {
            try
            {
                if (_fileSystem.Directory.Exists(path))
                {
                    SumFileDelete += DirectoryExtension.GetFolderSizeNoExtension(path, true);
                    _fileSystem.Directory.Delete(path, true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error at {nameof(DeleteDirectoryFromPath)} with message {ex.Message}");
            }

            return false;
        }

        public static void DeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);

                    Console.WriteLine($"===========================================");
                    Console.WriteLine($"Path: {path} Found and Deleted");
                    Console.WriteLine($"===========================================");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error at {nameof(DeleteDirectory)} with message {ex.Message}");
            }
        }
    }
}