/*
4. Асинхронная очередь (Producer/Consumer)
Написать:
продюсер — добавляет элементы в очередь с задержкой,
консюмер — асинхронно читает элементы и обрабатывает.
Подсказка: Channel<T> из System.Threading.Channels.
*/

using System;
using System.Threading.Tasks;
using System.Threading.Channels;

class Program
{
    static async Task Main()
    {
        Channel<int> channel = Channel.CreateUnbounded<int>();

        Task producerTask = ProducerAsync(channel.Writer);
        Task consumerTask = ConsumerAsync(channel.Reader);

        await Task.WhenAll(producerTask, consumerTask);

    }

    static async Task ProducerAsync(ChannelWriter<int> writer)
    {

        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(500);

            await writer.WriteAsync(i);
            Console.WriteLine($"Producer: {i}");
        }

        writer.Complete();
        Console.WriteLine("\nProducer закончил.");
    }

    static async Task ConsumerAsync(ChannelReader<int> reader)
    {

        await foreach (int item in reader.ReadAllAsync())
        {
            Console.WriteLine($"Consumer: {item}");

            await Task.Delay(300);
            Console.WriteLine($"Обработано Consumer: {item}\n");
        }
    }
}