/*
5. Асинхронный ретрай с экспоненциальной задержкой
Реализовать метод:
Task<T> RetryAsync<T>(Func<Task<T>> action, int maxAttempts);
При ошибке — повторять с паузой: 1s, 2s, 4s, …
*/

using System;
using System.Threading.Tasks;

class Program
{
    static async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxAttempts)
    {
        int attempt = 0;

        while (true)
        {
            attempt++;

            try
            {
                Console.WriteLine($"\nПопытка {attempt} из {maxAttempts}...");
                
                T result = await action();
                
                Console.WriteLine($"Успех на попытке {attempt}!");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");

                if (attempt >= maxAttempts)
                {
                    Console.WriteLine($"Все {maxAttempts} попыток исчерпаны");
                    throw;
                }
                int delaySeconds = (int)Math.Pow(2, attempt - 1);
                Console.WriteLine($"\nПовтор через {delaySeconds} сек...");
                
                await Task.Delay(delaySeconds * 1000);
            }
        }
    }

    static int callCount = 0;
    static async Task<string> UnstableMethodAsync()
    {
        callCount++;

        if (callCount < 4)
        {
            throw new Exception("Больше 4 попыток");
        }

        return "Выполнилось";
    }

    static async Task<int> AlwaysFailsAsync()
    {
        await Task.Delay(100);
        throw new Exception("Критическая ошибка");
    }

    static async Task Main()
    {
        callCount = 0;

        try
        {
            string result = await RetryAsync(UnstableMethodAsync, 5);
            Console.WriteLine($"\nРезультат: {result}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка: {ex.Message}\n");
        }


        try
        {
            int result = await RetryAsync(AlwaysFailsAsync, 3);
            Console.WriteLine($"\n  Результат: {result}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка: {ex.Message}\n");
        }
    }
}