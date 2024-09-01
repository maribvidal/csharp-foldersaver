namespace FolderSaver;

using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        string zipName = "zip";
        List<string> totalFilePaths;
        List<string> selectedFilePaths;

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
                Console.Write(" * Is this what you want? (Y/N): ");
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
            Console.WriteLine(" -- SELECT THE FILES YOU WANT TO COMPRESS -- ");

            // Enumerate directory files
            totalFilePaths = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).ToList();
            selectedFilePaths = SelectPaths(totalFilePaths);

            Console.Clear();
            Console.WriteLine(" -- NOW CREATING ZIP -- ");

            ZipManager zipManager = new ZipManager();
            zipManager.CreateZip(selectedFilePaths, zipName);

            Console.Clear();
            Console.WriteLine(" -- ZIP CREATED SUCCESFULLY -- ");
            Console.WriteLine(" * You can close the program");

        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.WriteLine(" -- ERROR -- ");
            Console.WriteLine($" * {ex.Message}");
        }
    }

    private static List<string> SelectPaths(List<string> paths)
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
        string? input = Console.ReadLine();
        while (String.IsNullOrEmpty(input))
            input = Console.ReadLine();
        return input;
    }
}