/*
3. Асинхронный поток с отменой
Создать IAsyncEnumerable<int> который реагирует на CancellationToken.
*/

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

class Program
{
    static async IAsyncEnumerable<int> GenerateNumbersAsync(int count, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (int i = 1; i <= count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(500, cancellationToken);

            Console.WriteLine($"Генерируем число {i}...");
            yield return i;
        }
    }

    static async Task Main()
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        cts.CancelAfter(2000);

        try
        {
            await foreach (int number in GenerateNumbersAsync(10, cts.Token))
            {
                Console.WriteLine($"Получено: {number}\n");
            }

            Console.WriteLine("✓");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Отмена");
        }
    }
}