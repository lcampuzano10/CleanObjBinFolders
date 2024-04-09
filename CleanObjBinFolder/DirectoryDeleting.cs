namespace CleanObjBinFolder
{
    public class DirectoryDeleting
    {
        public List<string> PathsToDelete { get; set; } = new();

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
                    PathsToDelete.Add(path);
                }
            }
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

        public static long GetFolderSize(string path, bool allDirectories, string extension)
        {
            var option = allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return new DirectoryInfo(path).EnumerateFiles("*" + extension, option).Sum(file => file.Length);
        }

        public long GetFolderSizeNoExtension(string path, bool allDirectories)
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

        public void DeletedFolderMessage(int count, string sizeDeleted)
        {
            Console.WriteLine($"Deleted {count} folders");
            Console.WriteLine($"Deleted {sizeDeleted} folders");
            Console.WriteLine("Finished. Press any key to close this window.");
        }
    }
}