# Лабораторная работа 1  
**Тема:** сериализация JSON, управление ресурсами и работа с файлами в C#

## 1. Краткое описание

В лабораторной реализованы:

1. Класс `Person` с атрибутами JSON и валидацией полей.
2. Класс `PersonSerializer` для сериализации/десериализации и работы с JSON‑файлами (синхронно и асинхронно).
3. Класс `FileResourceManager`, реализующий паттерн `IDisposable` для корректного управления файловыми ресурсами.
4. Консольное приложение с набором тестов для проверки реализованного функционала.


### Используемые технологии

| Технология   | Версия (минимальная/целевой фреймворк) |
|--------------|-----------------------------------------|
| C#           | 10+                                    |
| .NET SDK     | 6.0+ / 8.0+                            |

### Используемые пространства имён

- `System`
- `System.Text`
- `System.Text.Json`
- `System.Text.Json.Serialization`
- `System.Collections.Generic`
- `System.Threading.Tasks`
- `System.IO`

---

## 2. Класс Person и атрибуты JSON

### Задание

Реализован класс `Person` со следующими полями и свойствами:

- `FirstName` — имя  
- `LastName` — фамилия  
- `Age` — возраст  
- `FullName` — только для чтения (конкатенация `FirstName + " " + LastName`)  
- `IsAdult` — только для чтения (`Age >= 18`)  
- `Password` — помечено атрибутом `[JsonIgnore]` (не попадает в JSON)  
- `Id` — сериализуется с именем поля `"personId"` через `[JsonPropertyName("personId")]`  
- `BirthDate` — приватное поле `_birthDate` с публичным свойством и атрибутом `[JsonInclude]`  
- `Email` — свойство с валидацией (обязательное наличие символа `'@'`, иначе `ArgumentException`)  
- `PhoneNumber` — сериализуется под именем `"phone"` через `[JsonPropertyName("phone")]`


---

## 3. Менеджер сериализации PersonSerializer

### Задание

Класс `PersonSerializer` реализует:

1. `SerializeToJson(Person person)` — сериализация объекта `Person` в строку JSON.  
2. `DeserializeFromJson(string json)` — десериализация строки JSON в объект `Person`.  
3. `SaveToFile(Person person, string filePath)` — синхронное сохранение объекта в файл.  
4. `LoadFromFile(string filePath)` — синхронная загрузка объекта из файла.  
5. `SaveToFileAsync(Person person, string filePath)` — асинхронное сохранение объекта в файл.  
6. `LoadFromFileAsync(string filePath)` — асинхронная загрузка объекта из файла.  
7. `SaveListToFile(List<Person> people, string filePath)` — экспорт списка объектов в JSON‑файл.  
8. `LoadListFromFile(string filePath)` — импорт списка объектов из JSON‑файла.

Во всех методах используется `System.Text.Json`. Для чтения/записи файлов применяется кодировка UTF‑8.

В решении используется `JsonSerializerOptions` с:
- `WriteIndented = true` — для красивого форматирования JSON
- `Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping` — для корректного отображения символа `+` в телефоне
---

## 4. Класс FileResourceManager и IDisposable

### Задание

Реализован класс `FileResourceManager`, который управляет файловыми ресурсами и корректно освобождает их через `IDisposable`.

### Поля

- `FileStream _fileStream` — базовый поток для работы с файлом.
- `StreamWriter _writer` — текстовая запись.
- `StreamReader _reader` — текстовое чтение.
- `bool _disposed` — флаг, указывающий, что объект уже освобождён.
- `string _filePath` — путь к файлу.

### Основные методы

- Конструктор `FileResourceManager(string filepath)` — принимает путь к файлу.
- `OpenForWriting(string path)` — открытие файла для записи (создание/перезапись файла, `FileMode.Create`).
- `OpenForReading()` — открытие файла для чтения, с проверкой существования.
- `WriteLine(string text)` — запись строки в файл.
- `ReadAllText()` — чтение всего содержимого файла.
- `AppendText(string text)` — добавление текста в конец файла с использованием вложенных `using`.
- `GetFileInfo()` — возвращает `FileInfo` для файла (размер, дата создания и т.д.).
- `Dispose()` / `Dispose(bool disposing)` — корректное освобождение `StreamWriter`, `StreamReader`, `FileStream` и установка `_disposed = true`.  
- Финализатор `~FileResourceManager()` — вызывает `Dispose(false)` на случай, если пользователь не вызвал `Dispose()` явно.

Во всех публичных методах вызывается `ThrowIfDisposed()`, чтобы предотвратить использование освобождённого объекта.

---

## 5. Тестирование функционала

Логика тестирования реализована в `Program.Main`:

1. Создаётся директория `TestData` в текущем рабочем каталоге.
2. Определяются пути к файлам:
   - `person.json` — для одного объекта `Person`;
   - `people.json` — для списка объектов.

Выполняются тесты:

- `TestSerializeDeserialize(serializer)`  
  - Проверка сериализации в строку и последующей десериализации.
  - Проверяется, что поле `Password` не попадает в JSON (`[JsonIgnore]`).
  - Сравниваются основные свойства до и после десериализации.

- `TestSaveLoadFile(serializer, personFilePath)`  
  - Синхронное сохранение объекта в файл и загрузка обратно.
  - Проверяется создание файла и корректность данных после чтения.

- `TestSaveLoadFileAsync(serializer, personFilePath)`  
  - Асинхронное сохранение/загрузка.
  - Аналогичная проверка данных и существования файла.

- `TestSaveLoadList(serializer, listFilePath)`  
  - Сохранение списка из двух `Person` в файл и последующий импорт.
  - Сравнивается количество элементов и отдельные поля.

## 6. Инструкция по запуску

1. Скопировать код в `Program.cs`
2. Выполнить команду: dotnet run