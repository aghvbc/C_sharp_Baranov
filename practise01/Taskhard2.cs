using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string sourceDir = "Source";
        string backupDir = "Backup";        

        if (!Directory.Exists(sourceDir))
        {
            Directory.CreateDirectory(sourceDir);
            Console.WriteLine($"Создана папка: {sourceDir}");
        }

        if (!Directory.Exists(backupDir))
        {
            Directory.CreateDirectory(backupDir);
            Console.WriteLine($"Создана папка: {backupDir}");
        }

        string[] files = Directory.GetFiles(sourceDir);


        if (files.Length < 1)
        {
            string baseDir = Directory.GetCurrentDirectory();
            Console.WriteLine($"Создайте .txt и/или .json файл в папке Source по пути: {baseDir}/Source");
            return;
        }


        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).ToLower();

            if (extension == ".txt" || extension == ".json")
            {
                FileInfo info = new FileInfo(file);

                if (info.Length > 5 * 1024 * 1024)
                {
                    Console.WriteLine($"больше 5МБ: {info.Name}");
                    continue;
                }

                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);

                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string newFileName = $"{fileNameWithoutExt}_{date}{extension}";

                string destPath = Path.Combine(backupDir, newFileName);

                info.CopyTo(destPath, overwrite: true);

            }
        }
    }
}




