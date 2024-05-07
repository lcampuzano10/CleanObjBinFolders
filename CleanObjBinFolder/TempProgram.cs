using CleanObjBinFolder.Extensions;
using CleanObjBinFolder.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanObjBinFolder
{
    public class TempProgram
    {
        void Temp()
        {
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
                        RecursiveDirectory(pathToSearch);

                        foreach (var pathToDelete in listPathToDelete)
                        {
                            sumFileDelete += DirectoryExtension.GetFolderSizeNoExtension(pathToDelete, true);
                            DirectoryDeleting.DeleteDirectory(pathToDelete);
                        }

                        ApplicationPrompt.DeletedFolderMessage(listPathToDelete.Count, DirectoryExtension.FormatFileSize(sumFileDelete));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("There was an issue:");
                        Console.WriteLine(ex.Message);
                        ApplicationPrompt.DeletedFolderMessage(listPathToDelete.Count, DirectoryExtension.FormatFileSize(sumFileDelete));
                    }
                }
            }

            Console.ReadKey();
        }

        public void RecursiveDirectory(string parentPath)
        {
            if (Directory.Exists(parentPath))
            {
                var findParents = Directory.GetDirectories(parentPath).Where(s => !s.Contains(".vs")).ToList();

                foreach (var pathParent in findParents)
                {
                    var pathParentLower = pathParent.ToLower();
                    if (!(pathParentLower.Contains("angular") || pathParentLower.Contains("react")))
                    {
                        ListOfDirectoriesDelete(pathParent, "obj");
                        ListOfDirectoriesDelete(pathParent, "bin");
                    }

                    RecursiveDirectory(pathParent);
                }
            }
            else
            {
                Console.WriteLine("Directory does not exist");
            }
        }

        public void ListOfDirectoriesDelete(string pathParent, string directoryToFind)
        {
            var fullPath = Path.Combine(pathParent, directoryToFind);

            List<string> findDirectory = Directory.GetDirectories(pathParent).Where(x => x == fullPath).ToList();

            if (findDirectory.Count() > 0)
            {
                foreach (var path in findDirectory)
                {
                    //PathsToDelete.Add(path);
                }
            }
        }
    }
}
