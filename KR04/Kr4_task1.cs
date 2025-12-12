using System;
using System.Reflection;

// Создать класс Student с приватными полями и с помощью рефлексии
// получить доступ (вывести, изменить, вывести обновленные значения).

class Student
{
    private string name;
    private string group;

    public Student(string name, string group)
    {
        this.name = name;
        this.group = group;
    }
}

class Program
{
    static void Main()
    {
        Student student = new Student("Иван", "Группа 1");

        Type type = typeof(Student);

        FieldInfo nameField = type.GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo groupField = type.GetField("group", BindingFlags.NonPublic | BindingFlags.Instance);

        Console.WriteLine("До изменения:");
        Console.WriteLine("name  = " + nameField.GetValue(student));
        Console.WriteLine("group = " + groupField.GetValue(student));

        nameField.SetValue(student, "Пётр");
        groupField.SetValue(student, "Группа 2");

        Console.WriteLine("После:");
        Console.WriteLine("name  = " + nameField.GetValue(student));
        Console.WriteLine("group = " + groupField.GetValue(student));
    }
}