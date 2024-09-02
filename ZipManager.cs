namespace FolderSaver;

using System;
using System.Diagnostics;
using System.IO.Compression;

// Esta clase se encarga de comprimir los archivos

public class ZipManager
{
    // MÉTODO: CreateZip
    // - Parámetros: fileList (recibe una lista de strings que representan las rutas a los archivos)
    public void CreateZip(List<string> fileList, List<string> dirList, string zipName = "zip")
    {
        // Troubleshooting
        List<string> validFilePaths;
        List<string> validDirPaths;
        int invalidPaths = 0;

        if (fileList.Count < 1 && dirList.Count < 1)
            throw new ArgumentOutOfRangeException("At least one path is needed");
        
        // Check every file path
        validFilePaths = CheckFilePaths(fileList, ref invalidPaths);

        // Check every directory path
        validDirPaths = CheckDirPaths(dirList, ref invalidPaths);

        // Create the .zip
        if (validFilePaths.Count < 1 && validDirPaths.Count < 1)
            throw new InvalidOperationException("No valid path was received");

        zipName = (zipName == "zip") ? ("zip_generated_" + DateTime.UtcNow.Second + DateTime.UtcNow.Minute + DateTime.UtcNow.Hour + DateTime.UtcNow.Day) : zipName;
        using (FileStream zipFile = File.Open($"{zipName}.zip", FileMode.Create))
        {
            using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Update))
            {
                // First, the files
                foreach (string path in validFilePaths)
                {
                    var fileInfo = new FileInfo(path);
                    archive.CreateEntryFromFile(fileInfo.FullName,  fileInfo.Name);
                }
                // Then, the directories
                foreach (string path in validDirPaths)
                {
                    var dirInfo = new DirectoryInfo(path);
                    var dirFiles = dirInfo.GetFiles();
                    
                    foreach (FileInfo file in dirFiles)
                        archive.CreateEntryFromFile(file.FullName, dirInfo.Name + "/" + file.Name);
                }
            }
        }

        Debug.WriteLine($" -- COMPRESSION FINISHED: {invalidPaths} files weren't added");
    }

    private List<string> CheckFilePaths(List<string> fileList, ref int invalidPaths)
    {
        List<string> validFilePaths = new List<string>();

        foreach (string path in fileList)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                    validFilePaths.Add(path);
            }
            else
                invalidPaths++;
        }
        return validFilePaths;
    }

    private List<string> CheckDirPaths(List<string> dirList, ref int invalidPaths)
    {
        List<string> validDirPaths = new List<string>();

        foreach (string path in dirList)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if (Directory.Exists(path))
                    validDirPaths.Add(path);
            }
            else
                invalidPaths++;
        }
        return validDirPaths;
    }
}