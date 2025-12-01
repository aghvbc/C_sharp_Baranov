using System;

string dirPath = "/home/kirill/Документы/VSCODE/cs-test"; 

int fileCount = 0;
int dirCount = 0;
long totalSize = 0;

void Traverse(string path)
{
    try
    {
        string[] subdirs = Directory.GetDirectories(path);
        foreach (string dir in subdirs)
        {
            dirCount++;
            Traverse(dir);
        }

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            fileCount++;

            FileInfo fileInfo = new FileInfo(file);
            long size = fileInfo.Length;
            totalSize += size;

            if (size > 10 * 1024 * 1024)
            {
                Console.WriteLine($"Большой файл: {file} ({size / (1024.0 * 1024.0):0.00} МБ)");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Ошибка доступа] {path}: {ex.Message}");
    }
}

if (Directory.Exists(dirPath))
{
    Traverse(dirPath);

    Console.WriteLine();
    Console.WriteLine("Результаты:");
    Console.WriteLine("Файлов: " + fileCount);
    Console.WriteLine("Папок: " + dirCount);
    Console.WriteLine($"Общий размер: {(totalSize / 1024.0 / 1024.0):0.00} МБ");
}
else
{
    Console.WriteLine("Директория не найдена: " + dirPath);
}