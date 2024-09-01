namespace FolderSaver;

using System;
using System.Diagnostics;
using System.IO.Compression;

// Esta clase se encarga de comprimir los archivos

public class ZipManager
{
    // MÉTODO: CreateZip
    // - Parámetros: fileList (recibe una lista de strings que representan las rutas a los archivos)
    public void CreateZip(List<string> fileList, string zipName = "zip")
    {
        // Troubleshooting
        List<string> validPaths;
        int invalidPaths = 0;

        if (fileList.Count < 1)
            throw new ArgumentOutOfRangeException("At least one path is needed");
        
        // Check every path
        validPaths = CheckPaths(fileList, ref invalidPaths);

        // Create the .zip
        if (validPaths.Count < 1)
            throw new InvalidOperationException("No valid path was received");

        zipName = (zipName == "zip") ? ("zip_generated_" + DateTime.UtcNow.Second + DateTime.UtcNow.Minute + DateTime.UtcNow.Hour + DateTime.UtcNow.Day) : zipName;
        using (FileStream zipFile = File.Open($"{zipName}.zip", FileMode.Create))
        {
            using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Update))
            {
                foreach (string path in validPaths)
                {
                    var fileInfo = new FileInfo(path);
                    archive.CreateEntryFromFile(fileInfo.FullName,  fileInfo.Name);
                }
            }
        }

        Debug.WriteLine($" -- COMPRESSION FINISHED: {invalidPaths} files weren't added");
    }

    private List<string> CheckPaths(List<string> fileList, ref int invalidPaths)
    {
        List<string> validPaths = new List<string>();
        foreach (string path in fileList)
        {
            if (!String.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                    validPaths.Add(path);
            }
            else
                invalidPaths++;
        }
        return validPaths;
    }
}