using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Thread t1 = new Thread(Print_From_Zero_To_Hundred);
        Thread t2 = new Thread(Print_From_A_To_Z);

        t1.Start();
        t2.Start();
    }

    static void Print_From_A_To_Z()
    {
        for (char c = 'a'; c <= 'z'; c++)
        {
            Console.WriteLine(c);
        }
    }

    static void Print_From_Zero_To_Hundred()
    {
        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine(i);
        }
    }

    // Вывод перемешивается, это зависит от того когда потоки захотят выполнятся(ну процессор освободит ресурсы)
}