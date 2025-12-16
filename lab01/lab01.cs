using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }

    public string FullName => $"{FirstName} {LastName}";
    public bool IsAdult => Age >= 18;

    [JsonIgnore]
    public string Password { get; set; }

    [JsonPropertyName("personId")]
    public string Id { get; set; }

    [JsonInclude]
    private DateTime _birthDate;
    public DateTime BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }
    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrEmpty(value) || !value.Contains('@'))
                throw new ArgumentException("Email must contain '@'");
            _email = value;
        }
    }

    [JsonPropertyName("phone")]
    public string PhoneNumber { get; set; }

    public Person(string fname, string lname, int age, string pswd, string id, string phone, string email)
    {
        FirstName = fname;
        LastName = lname;
        Age = age;
        Password = pswd;
        PhoneNumber = phone;
        Email = email;
    }

    public Person() { }
}

public class PersonSerializer 
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    // 1. Сериализация в строку
    public string SerializeToJson(Person person)
    {
        return JsonSerializer.Serialize(person, _options);
    }

    // 2. Десериализация из строки
    public Person DeserializeFromJson(string json)
    {
        return JsonSerializer.Deserialize<Person>(json);
    }

    // 3. Сохранение в файл (синхронно)
    public void SaveToFile(Person person, string filePath)
    {
        string json = JsonSerializer.Serialize(person, _options);
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }
    
    // 4. Загрузка из файла (синхронно)
    public Person LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл не найден", filePath);
            
        string json = File.ReadAllText(filePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<Person>(json);
    }

    // 5. Сохранение в файл (асинхронно)
    public async Task SaveToFileAsync(Person person, string filePath)
    {
        string json = JsonSerializer.Serialize(person, _options);
        await File.WriteAllTextAsync(filePath, json, Encoding.UTF8);
    }

    // 6. Загрузка из файла (асинхронно)
    public async Task<Person> LoadFromFileAsync(string filePath)
    {
        string json = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<Person>(json);
    }

    // 7. Экспорт нескольких объектов в файл
    public void SaveListToFile(List<Person> people, string filePath)
    {
        string json = JsonSerializer.Serialize(people, _options);
        File.WriteAllText(filePath, json, Encoding.UTF8);
    }

    // 8. Импорт из файла
    public List<Person> LoadListFromFile(string filePath)
    {
        string json = File.ReadAllText(filePath, Encoding.UTF8);
        return JsonSerializer.Deserialize<List<Person>>(json);
    }
}

public class FileResourceManager : IDisposable
{
    private FileStream _fileStream { get; set; }
    private StreamWriter _writer { get; set; }
    private StreamReader _reader { get; set; }
    private bool _disposed { get; set; }
    private string _filePath { get; set; }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(FileResourceManager));
    }

    public FileResourceManager(string filepath)
    {
        _filePath = filepath;
    }

    public void OpenForWriting(string path)
    {
        _fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
        _writer = new StreamWriter(_fileStream, Encoding.UTF8);
    }

    public void OpenForReading()
    {
        ThrowIfDisposed();
        if (!File.Exists(_filePath))
            throw new FileNotFoundException("Файл не найден", _filePath);
        _fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        _reader = new StreamReader(_fileStream, Encoding.UTF8);
    }

    public void WriteLine(string text)
    {
        ThrowIfDisposed();
        _writer?.WriteLine(text);
    }

    public string ReadAllText()
    {
        ThrowIfDisposed();
        return _reader?.ReadToEnd() ?? string.Empty;
    }

    public void AppendText(string text)
    {
        ThrowIfDisposed();
        using var fs = new FileStream(_filePath, FileMode.Append, FileAccess.Write);
        using var writer = new StreamWriter(fs, Encoding.UTF8);
        writer.Write(text);
    }

    public FileInfo GetFileInfo()
    {
        ThrowIfDisposed();

        var fileInfo = new FileInfo(_filePath);
        if (!fileInfo.Exists)
            throw new FileNotFoundException("File not found", _filePath);

        return fileInfo;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _writer?.Dispose();
                _reader?.Dispose();
                _fileStream?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FileResourceManager()
    {
        Dispose(false);
    }

}

class Program
{
    static async Task Main()
    {
        var serializer = new PersonSerializer();

        string baseDir   = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
        Directory.CreateDirectory(baseDir);

        string personFilePath = Path.Combine(baseDir, "person.json");
        string listFilePath   = Path.Combine(baseDir, "people.json");

        try
        {
            TestSerializeDeserialize(serializer);
            TestSaveLoadFile(serializer, personFilePath);
            await TestSaveLoadFileAsync(serializer, personFilePath);
            TestSaveLoadList(serializer, listFilePath);

            Console.WriteLine("=== Все тесты успешно пройдены ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    // ---------- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ТЕСТОВ ----------

    static Person CreateSamplePerson()
    {
        return new Person(
            fname: "Ivan",           
            lname: "Ivanov",         
            age: 25,
            pswd: "secret123",       
            id: "p1",
            phone: "+7-999-000-00-00",
            email: "ivan.ivanov@example.com")
        {
            BirthDate = new DateTime(1999, 1, 1)
        };
    }

    static void AssertEqual<T>(T expected, T actual, string message)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new Exception($"Assert failed: {message}. Expected: {expected}, Actual: {actual}");
    }

    // 1) Тест сериализации/десериализации в строку
    static void TestSerializeDeserialize(PersonSerializer serializer)
    {
        Console.WriteLine("-> TestSerializeDeserialize");

        var person = CreateSamplePerson();

        string json = serializer.SerializeToJson(person);
        
        if (string.IsNullOrWhiteSpace(json))
            throw new Exception("SerializeToJson вернул пустую строку");

        // Проверяем, что пароль не попал в JSON
        if (json.Contains("secret123", StringComparison.Ordinal))
            throw new Exception("[JsonIgnore] не сработал: пароль попал в JSON");

        // Проверяем, что WriteIndented работает (JSON содержит переносы строк)
        if (!json.Contains('\n'))
            throw new Exception("WriteIndented не работает: JSON не содержит переносов строк");
        
        // Проверяем наличие отступов (пробелы в начале строки)
        if (!json.Contains("\n  "))
            throw new Exception("WriteIndented не работает: JSON не содержит отступов");

        Console.WriteLine("JSON (форматированный):");
        Console.WriteLine(json);
        Console.WriteLine();

        var restored = serializer.DeserializeFromJson(json);
        if (restored == null)
            throw new Exception("DeserializeFromJson вернул null");

        AssertEqual(person.FirstName,   restored.FirstName,   "FirstName после десериализации");
        AssertEqual(person.LastName,    restored.LastName,    "LastName после десериализации");
        AssertEqual(person.Age,         restored.Age,         "Age после десериализации");
        AssertEqual(person.Id,          restored.Id,          "Id после десериализации");
        AssertEqual(person.PhoneNumber, restored.PhoneNumber, "PhoneNumber после десериализации");
        AssertEqual(person.Email,       restored.Email,       "Email после десериализации");
        AssertEqual(person.BirthDate,   restored.BirthDate,   "BirthDate после десериализации");

        Console.WriteLine("TestSerializeDeserialize: OK (включая проверку WriteIndented)");
    }

    // 2) Тест синхронного сохранения/загрузки одного объекта
    static void TestSaveLoadFile(PersonSerializer serializer, string filePath)
    {
        Console.WriteLine("-> TestSaveLoadFile");

        var person = CreateSamplePerson();

        serializer.SaveToFile(person, filePath);

        if (!File.Exists(filePath))
            throw new Exception("Файл не был создан методом SaveToFile");

        var restored = serializer.LoadFromFile(filePath);
        if (restored == null)
            throw new Exception("LoadFromFile вернул null");

        AssertEqual(person.FirstName, restored.FirstName, "FirstName после LoadFromFile");
        AssertEqual(person.LastName,  restored.LastName,  "LastName после LoadFromFile");
        AssertEqual(person.Age,       restored.Age,       "Age после LoadFromFile");

        Console.WriteLine($"Файл {filePath} успешно создан и прочитан.");
        Console.WriteLine("TestSaveLoadFile: OK");
    }

    // 3) Тест асинхронного сохранения/загрузки одного объекта
    static async Task TestSaveLoadFileAsync(PersonSerializer serializer, string filePath)
    {
        Console.WriteLine("-> TestSaveLoadFileAsync");

        var person = CreateSamplePerson();

        await serializer.SaveToFileAsync(person, filePath);

        if (!File.Exists(filePath))
            throw new Exception("Файл не был создан методом SaveToFileAsync");

        var restored = await serializer.LoadFromFileAsync(filePath);
        if (restored == null)
            throw new Exception("LoadFromFileAsync вернул null");

        AssertEqual(person.FirstName, restored.FirstName, "FirstName после LoadFromFileAsync");
        AssertEqual(person.LastName,  restored.LastName,  "LastName после LoadFromFileAsync");
        AssertEqual(person.Age,       restored.Age,       "Age после LoadFromFileAsync");

        Console.WriteLine("TestSaveLoadFileAsync: ОК");
    }

    // 4) Тест сохранения/загрузки списка объектов
    static void TestSaveLoadList(PersonSerializer serializer, string filePath)
    {
        Console.WriteLine("-> TestSaveLoadList");

        var people = new List<Person>
        {
            CreateSamplePerson(),
            new Person("Petr", "Petrov", 30, "pwd2", "p2", "+7-999-111-11-11", "petr.petrov@example.com")
            {
                BirthDate = new DateTime(1994, 2, 2)
            }
        };

        serializer.SaveListToFile(people, filePath);

        if (!File.Exists(filePath))
            throw new Exception("Файл со списком не был создан методом SaveListToFile");

        var restoredList = serializer.LoadListFromFile(filePath);
        if (restoredList == null)
            throw new Exception("LoadListFromFile вернул null");

        AssertEqual(people.Count, restoredList.Count, "Количество элементов в списке после LoadListFromFile");

        AssertEqual(people[0].FirstName, restoredList[0].FirstName, "FirstName [0] после LoadListFromFile");
        AssertEqual(people[1].FirstName, restoredList[1].FirstName, "FirstName [1] после LoadListFromFile");

        Console.WriteLine("TestSaveLoadList: OK");
    }
}