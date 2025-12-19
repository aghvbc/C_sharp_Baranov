// 5. Нормализация сложных имён и фамилий
// Вход:
// " aНТоН пЕтрОв-ИВАНОв "
// Требуется:
// удалить лишние пробелы
// каждую часть имени привести к виду: первая буква — заглавная, остальные — строчные
// сохранение дефисов и апострофов
// поддержка русских и латинских букв
// Вывод:
// "Антон Петров-Иванов"


using System;
using System.Data.SqlTypes;
using System.Text;

class Program
{
    static void Main()
    {
        string mystring = "АнТоН pOn John-ИваНОВ";

        bool new_word = true;
        StringBuilder res = new StringBuilder();
        
        
        foreach (char letter in mystring)
        {
            if (char.IsLetter(letter))
            {
                if (new_word)
                {
                    res.Append(char.ToUpper(letter));
                    new_word = false;

                }
                else
                {
                    res.Append(char.ToLower(letter));
                }

            }
            else
            {
                res.Append(letter);
                new_word = true;

            }

        }
        Console.WriteLine(res.ToString());
    }

}