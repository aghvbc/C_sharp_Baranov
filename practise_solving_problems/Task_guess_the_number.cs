using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;


class Program
{
    static void Main()
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 101);

        Console.WriteLine("Угадай число от 1 до 100:");
        Console.WriteLine(randomNumber);

        int count = 0;
        bool f = true;
        
        while (f)
        {
            string n = Console.ReadLine();
            int num;
            if (int.TryParse(n, out num))
            {
                if (num > 100)
                {
                    Console.WriteLine("-_-");
                }
                else if(num < 1)
                {
                    Console.WriteLine("-_-");
                }
                else
                {
                    if (num < randomNumber)
                    {
                        Console.WriteLine("Больше");
                        count++;
                    }
                    else if (num > randomNumber)
                    {
                        Console.WriteLine("Меньше");
                        count++;
                    }
                    else
                    {
                        count++;
                        Console.WriteLine($"Вы угадали число, количество попыток: {count}");
                        f = false;
                    }
                }
            }
            else
            {
                Console.WriteLine("Не удалось преобразовать строку.");
            }
        }
    }
}

