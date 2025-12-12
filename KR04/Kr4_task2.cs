// написать программу создающую SOH и LOH

using System;

class Program
{
    static void Main()
    {         
        byte[] soh = new byte[60000];
        byte[] loh = new byte[90000]; 
        

        Console.WriteLine(GC.GetGeneration(soh)); // 0
        Console.WriteLine(GC.GetGeneration(loh)); // 2 - автоматически тк в большая куча

    }
}
