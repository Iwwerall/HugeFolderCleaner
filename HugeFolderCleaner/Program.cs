using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HugeFolderCleaner
{
    class Program
    {
        static int FileCount = 0;
        static long TotalSize = 0;
        static int FailedCount = 0;
        static bool DeletingFinished = false;
        static bool StopDeleting = false;

        static void Main(string[] args)
        {
            if(args.Length != 3)
            {
                Console.WriteLine("Usage: ");
                Console.WriteLine("HugeFolderCleaner.exe <Path> <SearchPattern> <MaxAgeHours> ");
                Console.WriteLine(@"Path: c:\ProgramData\Microsoftt\Crypto\RSA\MachineKeys");
                Console.WriteLine(@"SearchPattern: *.log ");
                Console.WriteLine(@"MaxAgeHours: 72 ");
                Console.WriteLine(@"This will delete all *.log files forom the path if the weren't changed for at least 72 hours.");
            }
            else
            {
                int MaxAgeHours = 72;
                if (!int.TryParse(args[2], out MaxAgeHours))
                {
                    Console.WriteLine(args[2] + " is not a valid value for MaxAgeHours");
                }
                else if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine(args[0] + " does not exists");
                }
                else
                {
                    Console.Clear();
                    new Thread(() =>
                    {
                        DeleteFiles(args[0], args[1], MaxAgeHours);
                        DeletingFinished = true;
                    }).Start();

                    new Thread(() =>
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        do
                        {
                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("Files Deleted: " + FileCount.ToString());
                            Console.WriteLine("Failed Files: " + FailedCount.ToString());
                            Console.WriteLine("Total Deleted: " + ((double)TotalSize / 1024 / 1024).ToString("N2") + "MB");
                            Console.WriteLine("Average Speed: " + ((double)TotalSize / sw.ElapsedMilliseconds / 1024).ToString("N2") + "MB/sec");
                            Thread.Sleep(100);

                        } while (!DeletingFinished);

                    }).Start();
                }
            }
        }

        static void DeleteFiles(string DirectoryToCheck, string SearchPattern, int MaxAgeHours)
        {
            try
            {
                if (Directory.Exists(DirectoryToCheck))
                {
                    // Folder, so go deeper
                    foreach (string EchteFile in Directory.EnumerateFiles(DirectoryToCheck, SearchPattern))
                        DeleteFiles(EchteFile, SearchPattern, MaxAgeHours);
                    foreach (string EchteFile in Directory.EnumerateDirectories(DirectoryToCheck))
                        DeleteFiles(EchteFile, SearchPattern, MaxAgeHours);
                }
                else
                {
                    // Het is een file, in de lijst zetten
                    FileInfo fi = new FileInfo(DirectoryToCheck);

                    if ((DateTime.Now - fi.LastWriteTime).TotalHours >= MaxAgeHours)
                    {
                        try
                        {
                            File.Delete(DirectoryToCheck);
                            FileCount++;
                            TotalSize += fi.Length;
                        }
                        catch (Exception eee)
                        {
                            FailedCount++;
                        }
                    }
                }
            }
            catch (Exception eee)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(eee.Message + "\r\n" + eee.StackTrace);
                Console.ResetColor();
            }
        }
    }
}
