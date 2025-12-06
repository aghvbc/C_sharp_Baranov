// 6. Выделение контекста по регуляркам (RegEx lookbehind/lookahead)
// Дан текст работы программы. Нужно найти:
// все строки, где встречается слово ERROR
// 2 строки до
// 2 строки после
// То есть реализовать контекстный поиск на базе Regex с lookbehind и lookahead.
// Пример:
// Line 20: INFO Start  
// Line 21: INFO Init  
// Line 22: ERROR Fail to load  
// Line 23: INFO Retry  
// Line 24: INFO Finish
// Должно вернуть строки 20–24.

// using System;
// using System.Text.RegularExpressions;

// class Program
// {
//     static void Main()
//     {
//         string log = """
//             Line 001: DEBUG Starting application...
//             Line 002: INFO Loading configuration
//             Line 003: INFO Database connection established
//             Line 004: WARN Deprecated API used
//             Line 005: INFO User login: admin
//             Line 006: INFO Session created
//             Line 007: ERROR Failed to connect to payment gateway
//             Line 008: ERROR Timeout after 5000ms
//             Line 009: ERROR Transaction aborted
//             Line 010: INFO Sending alert to administrator
//             Line 011: INFO System recovery initiated
//             Line 012: INFO Application restarted successfully
//             Line 013: DEBUG Cache cleared
//             Line 014: INFO User login: guest
//             Line 015: ERROR Division by zero in module Calculator
//             Line 016: WARN High memory usage detected
//             Line 017: INFO Cleanup completed
//             Line 018: DEBUG End of log 
//            """;


//         // Разбор паттерна:
//         // (?<=                  - Начало Positive Lookbehind (просмотр назад)
//         //   (?<pre>             - Именованная группа "pre" для захвата строк ДО
//         //     (?:.*\r?\n){0,2}  - От 0 до 2 строк (нежадный захват строк с переносами)
//         //   )
//         // )
//         // .*ERROR.*             - Основное совпадение: строка, содержащая ERROR
//         // (?=                   - Начало Positive Lookahead (просмотр вперед)
//         //   (?<post>            - Именованная группа "post" для захвата строк ПОСЛЕ
//         //     (?:\r?\n.*){0,2}  - От 0 до 2 строк (начиная с переноса строки)
//         //   )
//         // )
        
//         string pattern = @"(?<=(?<pre>(?:.*\r?\n){0,2})).*ERROR.*(?=(?<post>(?:\r?\n.*){0,2}))";

//         Regex regex = new Regex(pattern, RegexOptions.Multiline);

//         MatchCollection matches = regex.Matches(log);


//         foreach (Match m in matches)
//         {
//             string contextBlock = m.Groups["pre"].Value + m.Value + m.Groups["post"].Value;

//             Console.WriteLine(contextBlock.Trim());
//         }
//     }
// }