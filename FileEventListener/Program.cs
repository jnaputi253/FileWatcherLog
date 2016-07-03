using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileEventListener
{
    class Program
    {
        static void Main()
        {
            
            string path = GetFilePath();
            FileWatcher watcher = new FileWatcher(path);

            Console.WriteLine("Press 'q' to Quit");

            while (Console.ReadLine() != "q") ;
            watcher.Cleanup();
        }

        private static string GetFilePath()
        {
            string path = "";

            while (true)
            {
                Console.Write("Enter a file path: ");
                path = Console.ReadLine();

                try
                {
                    if (path != null && !Directory.Exists(path))
                        throw new DirectoryNotFoundException("\nDirectory supplied could not be found");

                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Please enter a valid directory\n");
                }
            }

            if (path != null && path[path.Length - 1].ToString().Equals(@"\")) return path;
            var builder = new StringBuilder(path);
            builder.Append(@"\");

            return builder.ToString();
        }
    }
}
