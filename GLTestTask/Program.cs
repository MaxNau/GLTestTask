using System;
using System.IO;
using WordSplitter;

namespace GLTestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string inPath = CheckAndGetFilePath("Path to file you want to read: ");
            string outPath = CheckAndGetFilePath("Path to file you want to write. File will be rewriten: ");

            IWordSplitter ws = new WordSplitter.WordSplitter();

            ws.Write(outPath, ws.ReadFile(inPath));
        }

        private static string CheckAndGetFilePath(string message)
        {
            string path = null;
            bool isValidPath = false;
            while (!isValidPath)
            {
                Console.Write(message);
                path = Console.ReadLine();
                string directory = null;
                try
                {
                    directory = Path.GetDirectoryName(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " Provide a valid file path");
                }

                if (Directory.Exists(directory))
                {
                    isValidPath = true;
                }
                else
                {
                    Console.WriteLine(" Provide a valid file path");
                }
            }

            return path;
        }
    }
}
