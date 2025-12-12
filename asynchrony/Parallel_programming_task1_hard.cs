/*
1. Обработка исключений в async/await
Симулировать метод, который иногда выбрасывает исключение.
Запустить 5 раз параллельно, корректно обработать исключения и вывести:
успешные результаты,
ошибки.
*/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static Random random = new Random();
    static async Task<string> UnstableOperationAsync(int operationId)
    {
        await Task.Delay(random.Next(500, 2000));

        if (random.Next(10) < 4)
        {
            throw new Exception($"Операция {operationId} завершилась с ошибкой!");
        }

        return $"Операция {operationId} выполнена успешно! Результат: {random.Next(100, 999)}";
    }

    static async Task Main()
    {

        List<string> successes = new List<string>();

        List<string> errors = new List<string>();

        Task<string>[] tasks = new Task<string>[5];


        for (int i = 1; i <= 5; i++)
        {
            int operationId = i; 
            tasks[i - 1] = UnstableOperationAsync(operationId);
        }

        foreach (var task in tasks)
        {
            try
            {
                string result = await task;
                successes.Add(result);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
        }


        Console.WriteLine($"Успешных операций: {successes.Count}");
        if (successes.Count > 0)
        {
            foreach (string success in successes)
            {
                Console.WriteLine($"{success}");
            }
        }
        else
        {
            Console.WriteLine("нет успешных операций");
        }

        Console.WriteLine();

        Console.WriteLine($"Ошибок: {errors.Count}");
        if (errors.Count > 0)
        {
            foreach (string error in errors)
            {
                Console.WriteLine($"{error}");
            }
        }
        else
        {
            Console.WriteLine("нет ошибок");
        }

    }
}