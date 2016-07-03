using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileEventListener
{
    class FileWatcher
    {
        private readonly FileSystemWatcher mFileSystemWatcher;
        private string mPathToLogFile;
        private StreamWriter mWriter;

        public FileWatcher(string path)
        {
            mFileSystemWatcher = new FileSystemWatcher(path);
            GenerateLogFile();
            WireupEvents();
        }

        private void GenerateLogFile()
        {
            mPathToLogFile = mFileSystemWatcher.Path + "log.txt";
            mWriter = File.Exists(mPathToLogFile) ? new StreamWriter(mPathToLogFile, true) : new StreamWriter(File.Create(mPathToLogFile));
            InsertInitialLogData();
        }

        private void InsertInitialLogData()
        {
            mWriter.WriteLine($"Directory Monitered => {mFileSystemWatcher.Path}");
            mWriter.WriteLine();
        }

        private void WireupEvents()
        {
            mFileSystemWatcher.Changed += OnChanged;
            mFileSystemWatcher.Created += OnChanged;
            mFileSystemWatcher.Deleted += OnChanged;
            mFileSystemWatcher.Renamed += OnRenamed;

            mFileSystemWatcher.EnableRaisingEvents = true;
            mFileSystemWatcher.IncludeSubdirectories = true;
        }

        private void OnChanged(object o, FileSystemEventArgs e)
        {
            if (e.FullPath.Equals(mPathToLogFile)) return;

            string info = $"File: {e.Name} => {e.ChangeType}";
            Console.WriteLine(info);

            LogChange(e);
        }

        private void OnRenamed(object o, RenamedEventArgs e)
        {
            Console.WriteLine("File {0} renamed to {1}", e.OldFullPath, e.FullPath);
            LogRename(e);
        }

        private void LogChange(FileSystemEventArgs e)
        {
            mWriter.WriteLine($"File Affected => {e.Name}");
            mWriter.WriteLine($"Full Path to Item => {e.FullPath}");
            mWriter.WriteLine($"Event Type => {e.ChangeType}");
            mWriter.WriteLine($"Event => {e.Name}");
            mWriter.WriteLine($"Time Detail => {DateTime.Now.ToLongDateString()}");
            mWriter.WriteLine("----------------SPACE---------------------");
        }

        private void LogRename(RenamedEventArgs e)
        {
            mWriter.WriteLine($"Absolute Path to Affected Directory => {e.FullPath}");
            mWriter.WriteLine($"Event => {e.OldFullPath} renamed to {e.FullPath}");
            mWriter.WriteLine($"Time Detail => {DateTime.Now.ToLongDateString()}");
            mWriter.WriteLine("----------------SPACE---------------------");
        }

        public void Cleanup()
        {
            mWriter.WriteLine();
            mWriter.WriteLine("==============================================");
            mWriter.WriteLine("=============END OF CURRENT LOG=============");
            mWriter.WriteLine("==============================================");
            mWriter.WriteLine();
            mWriter.WriteLine();

            mWriter.Close();
        }
    }
}
