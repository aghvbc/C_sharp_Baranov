// Задачка:
// 4. Написать класс с финализатором и методом Dispose
// class LogWriter : IDisposable
// {
//     FileStream file;
// }

// 1. Сделать задачу 4
// 2. Создайте массив объектов и с помощью GC.GetGeneration покажите, как они переходят между поколениями.
// 3. Ответить на вопрос



using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

class LogWriter : IDisposable
{
    FileStream file;
    private bool disposed = false;

    public LogWriter(string path)
    {
        file = new FileStream(path, FileMode.OpenOrCreate);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void read_file()
    {

        file.Seek(0, SeekOrigin.Begin); 
        byte[] buffer = new byte[file.Length];
        file.Read(buffer, 0, buffer.Length);
        
        string content = Encoding.UTF8.GetString(buffer);
        Console.WriteLine("Содержимое файла:");
        Console.WriteLine(content);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                file.Dispose(); 
            }
            disposed = true;
        }
    }

    ~LogWriter()
    {
        Dispose(false);
    }


}


class Program
{
    static void Main()
    {
        List<LogWriter> lw_list = new List<LogWriter>()
        {
            new LogWriter("/home/kirill/Документы/VSCODE/cs-test/error.txt"),
            new LogWriter("/home/kirill/Документы/VSCODE/cs-test/error.txt"),
            new LogWriter("/home/kirill/Документы/VSCODE/cs-test/error.txt"),

        };

        LogWriter lw = new LogWriter("/home/kirill/Документы/VSCODE/cs-test/error.txt");
        lw.read_file();
        Console.WriteLine($"Поколение lw: {GC.GetGeneration(lw)}");

        foreach(var item in lw_list)
        {
            item.read_file();
            Console.WriteLine($"Поколение из list: {GC.GetGeneration(item)}");
            
        }

        // Форсируем GC
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        foreach(var item in lw_list)
        {
            item.read_file();
            Console.WriteLine($"Поколение из list: {GC.GetGeneration(item)}");
            
        }

    }

}

