﻿namespace FolderSaver;

using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        bool runtime = true;
        string zipName = "zip";

        List<string> totalFilePaths;
        List<string> totalDirPaths;
        List<string> selectedFilePaths;
        List<string> selectedDirPaths;

        // While the program is running...
        while (runtime) 
        {
            Console.Clear();
            Console.WriteLine(" == ZIPFILE MANAGER (v0.1) == ");

            try
            {
                // Set new working directory if an argument was passed
                if (args.Length > 0)
                {
                    if (!Directory.Exists(args[0]))
                        throw new InvalidOperationException("That directory doesn't exist");

                    Directory.SetCurrentDirectory(args[0]);

                    if (args.Length > 1)
                        zipName = args[1];
                }
                else
                {
                    Console.WriteLine(" * If you're not providing any arguments, current directory will be used");
                    Console.WriteLine(" * Is this what you want? (Y/N) ");
                    if (UserInput() == "N")
                    {
                        Console.WriteLine(" * Enter a valid directory path: ");
                        string dirPath = UserInput();
                        while (!Directory.Exists(dirPath))
                            dirPath = UserInput();
                        Directory.SetCurrentDirectory(dirPath);    
                    }
                }

                Console.Clear();
                Console.WriteLine(" == ZIPFILE MANAGER (v0.1) == ");
                Console.WriteLine(" -- SELECT THE FILES YOU WANT TO COMPRESS -- ");

                // Enumerate directory files
                totalFilePaths = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).ToList();
                selectedFilePaths = SelectFilePaths(totalFilePaths);

                // Enumerate directory paths
                totalDirPaths = Directory.EnumerateDirectories(Directory.GetCurrentDirectory()).ToList();
                selectedDirPaths = SelectDirPaths(totalDirPaths);

                Console.Clear();
                Console.WriteLine(" == ZIPFILE MANAGER (v0.1) == ");
                Console.WriteLine(" -- NOW CREATING ZIP -- ");

                ZipManager zipManager = new ZipManager();
                zipManager.CreateZip(selectedFilePaths, selectedDirPaths, zipName);

                Console.Clear();
                Console.WriteLine(" == ZIPFILE MANAGER (v0.1) == ");
                Console.WriteLine(" -- ZIP CREATED SUCCESFULLY -- ");
                Console.WriteLine(" * You can close the program");
                runtime = false;

            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Clear();
                Console.WriteLine(" == ZIPFILE MANAGER (v0.1) == ");
                Console.WriteLine(" -- ERROR -- ");
                Console.WriteLine($" * One path is at least required to create a .zip");
                Console.WriteLine(" * \n * Do you wish to stop the program? (Y/N)");

                if (UserInput() == "Y") 
                {
                    runtime = false;
                    Console.WriteLine(" * Closing app");
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(" == ZIPFILE MANAGER (v0.1) == ");
                Console.WriteLine(" -- ERROR -- ");
                Console.WriteLine($" * {ex.Message}");
                runtime = false;
            }
        }
    }

    private static List<string> SelectFilePaths(List<string> paths)
    {
        List<string> newPaths = new List<string>();

        foreach (string path in paths)
        {
            Console.Write($" // {path} / ADD? (Y/N): ");
            if (UserInput() == "Y")
                newPaths.Add(path);
        }

        return newPaths;
    }

    private static List<string> SelectDirPaths(List<string> paths)
    {
        List<string> newPaths = new List<string>();

        foreach (string path in paths)
        {
            Console.Write($" // {path} / ADD? (Y/N): ");
            if (UserInput() == "Y")
                newPaths.Add(path);
        }

        return newPaths;
    }

    private static string UserInput()
    {
        Console.Write(" > ");
        string? input = Console.ReadLine();
        while (String.IsNullOrEmpty(input))
            input = Console.ReadLine();
        return input;
    }
}