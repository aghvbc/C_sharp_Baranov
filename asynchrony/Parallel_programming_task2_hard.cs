/*
2. Собственный асинхронный итератор
Реализовать метод:
async IAsyncEnumerable<int> GenerateNumbersAsync(int count);
Каждое число — через задержку 200 мс.
Вызвать через await foreach.
*/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            await Task.Delay(200);
            
            yield return i;
        }
    }

    static async Task Main()
    {
        await foreach (int number in GenerateNumbersAsync(10))
        {
            Console.WriteLine($"Получено число: {number}");
        }
    }
}