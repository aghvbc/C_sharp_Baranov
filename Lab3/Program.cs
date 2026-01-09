
using Lab3;

class Program {
    public static void Main()
    {
        
        
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("SIMPLE LIST<int>");

        var list = new SimpleList<int>();

        Console.WriteLine("\nДобавляем элементы: 10, 20, 30, 40, 50");
        list.Add(10);
        list.Add(20);
        list.Add(30);
        list.Add(40);
        list.Add(50);
        PrintList(list);

        Console.WriteLine("\nПоиск элемента 30:");
        Console.WriteLine($"IndexOf(30) = {list.IndexOf(30)}");
        Console.WriteLine($"Contains(30) = {list.Contains(30)}");
        Console.WriteLine($"Contains(999) = {list.Contains(999)}");

        Console.WriteLine("\nВставляем 25 на позицию 2:");
        list.Insert(2, 25);
        PrintList(list);

        Console.WriteLine("\nУдаляем элемент 30:");
        list.Remove(30);
        PrintList(list);

        Console.WriteLine("\nУдаляем элемент по индексу 0:");
        list.RemoveAt(0);
        PrintList(list);

        Console.WriteLine("\nИзменяем list[1] = 999:");
        list[1] = 999;
        PrintList(list);

        Console.WriteLine("\nПеребор через foreach:");
        Console.Write("   ");
        foreach (var item in list)
        {
            Console.Write($"[{item}] → ");
        }
        Console.WriteLine("конец");

        Console.WriteLine("\nОчищаем список:");
        list.Clear();
        Console.WriteLine($"Count = {list.Count}");

        Console.WriteLine("SIMPLE DICTIONARY<string, int>");

        var dict = new SimpleDictionary<string, int>();

        Console.WriteLine("\nДобавляем пары ключ-значение:");
        dict.Add("яблоки", 10);
        dict.Add("бананы", 25);
        dict.Add("апельсины", 15);
        dict["груши"] = 30;  
        PrintDict(dict);

        Console.WriteLine("\nПоиск по ключу:");
        Console.WriteLine($"dict[\"бананы\"] = {dict["бананы"]}");
        Console.WriteLine($"ContainsKey(\"яблоки\") = {dict.ContainsKey("яблоки")}");
        Console.WriteLine($"ContainsKey(\"манго\") = {dict.ContainsKey("манго")}");

        Console.WriteLine("\nTryGetValue(\"апельсины\"):");
        if (dict.TryGetValue("апельсины", out int value))
        {
            Console.WriteLine($"Найдено: {value}");
        }

        Console.WriteLine("\nИзменяем dict[\"бананы\"] = 100:");
        dict["бананы"] = 100;
        PrintDict(dict);

        Console.WriteLine("\nУдаляем \"яблоки\":");
        dict.Remove("яблоки");
        PrintDict(dict);

        Console.WriteLine("\nПолучаем все ключи и значения:");
        Console.Write("Ключи: ");
        foreach (var key in dict.Keys)
            Console.Write($"{key}, ");
        Console.WriteLine();

        Console.Write("Значения: ");
        foreach (var val in dict.Values)
            Console.Write($"{val}, ");
        Console.WriteLine();

        Console.WriteLine("DOUBLY LINKED LIST<string>");

        var linked = new DoublyLinkedList<string>();

        Console.WriteLine("\nДобавляем элементы в конец: A, B, C");
        linked.Add("A");
        linked.Add("B");
        linked.Add("C");
        PrintLinkedList(linked);

        Console.WriteLine("\nAddFirst(\"START\") - добавляем в начало:");
        linked.AddFirst("START");
        PrintLinkedList(linked);

        Console.WriteLine("\nAddLast(\"END\") - добавляем в конец:");
        linked.AddLast("END");
        PrintLinkedList(linked);

        Console.WriteLine("\nInsert(2, \"MIDDLE\") - вставляем на позицию 2:");
        linked.Insert(2, "MIDDLE");
        PrintLinkedList(linked);

        Console.WriteLine("\nДоступ по индексу:");
        Console.WriteLine($"linked[0] = \"{linked[0]}\"");
        Console.WriteLine($"linked[3] = \"{linked[3]}\"");
        Console.WriteLine($"linked[{linked.Count - 1}] (последний) = \"{linked[linked.Count - 1]}\"");

        Console.WriteLine("\nУдаляем \"B\":");
        linked.Remove("B");
        PrintLinkedList(linked);

        Console.WriteLine("\nRemoveAt(1) - удаляем по индексу 1:");
        linked.RemoveAt(1);
        PrintLinkedList(linked);

        void PrintList<T>(SimpleList<T> lst)
        {
            Console.Write($"   List (Count={lst.Count}): [ ");
            for (int i = 0; i < lst.Count; i++)
            {
                Console.Write(lst[i]);
                if (i < lst.Count - 1) Console.Write(", ");
            }
            Console.WriteLine(" ]");
        }

        void PrintDict<TKey, TValue>(SimpleDictionary<TKey, TValue> d) where TKey : notnull
        {
            Console.WriteLine($"   Dictionary (Count={d.Count}):");
            foreach (var kvp in d)
            {
                Console.WriteLine($"      \"{kvp.Key}\" => {kvp.Value}");
            }
        }

        void PrintLinkedList<T>(DoublyLinkedList<T> ll)
        {
            Console.Write($"   LinkedList (Count={ll.Count}): ");
            Console.Write("null ⟷ ");
            foreach (var item in ll)
            {
                Console.Write($"[{item}] ⟷ ");
            }
            Console.WriteLine("null");
        }

    }
}