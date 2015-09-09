using System;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace FileBackgroundService
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            FolderUpdate();
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"Test1";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handlers. 
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            FolderUpdate();
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            FolderUpdate();
        }

        public static void FolderUpdate()
        {
            string sourcePath = @"Test1";
            string targetPath = @"Test2";

            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            string[] files = Directory.GetFiles(sourcePath);

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                string fullTargetPath = Path.Combine(targetPath, fileInfo.Name);

                if (File.Exists(fullTargetPath)) File.Delete(fullTargetPath);
                    File.Move(file, fullTargetPath);

                Console.WriteLine("Found: {0}, Moved To: {1}", file, Path.Combine(targetPath, fileInfo.Name));
            }
        }
    }
}
