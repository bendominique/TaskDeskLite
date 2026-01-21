namespace TaskDeskLite;

public static class ConsoleUi
{
    public static void Title(string text)
    {
        Console.WriteLine(text);
        Console.WriteLine(new string('=', text.Length));
        Console.WriteLine();
    }

    public static void Success(string msg)
    {
        Console.WriteLine($"\n✅ {msg}");
    }

    public static void Error(string msg)
    {
        Console.WriteLine($"\n❌ {msg}");
    }

    public static void Pause()
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

    public static string ReadString(string label)
    {
        while (true)
        {
            Console.Write(label);
            var s = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
            Console.WriteLine("Valor obrigatório. Tente novamente.");
        }
    }

    public static string? ReadOptionalString(string label)
    {
        Console.Write(label);
        var s = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(s)) return null;
        return s.Trim();
    }

    public static int ReadInt(string label, int min, int max)
    {
        while (true)
        {
            Console.Write(label);
            var s = Console.ReadLine();
            if (int.TryParse(s, out var n) && n >= min && n <= max) return n;
            Console.WriteLine($"Digite um número entre {min} e {max}.");
        }
    }

    public static Guid ReadGuid(string label)
    {
        while (true)
        {
            Console.Write(label);
            var s = Console.ReadLine();
            if (Guid.TryParse(s, out var id)) return id;
            Console.WriteLine("GUID inválido. Exemplo: 3f2504e0-4f89-11d3-9a0c-0305e82c3301");
        }
    }

    public static DateTime? ReadOptionalDate(string label)
    {
        while (true)
        {
            Console.Write(label);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return null;

            if (DateTime.TryParseExact(s.Trim(), "dd/MM/yyyy", null,
                System.Globalization.DateTimeStyles.None, out var dt))
                return dt.Date;

            Console.WriteLine("Data inválida. Use dd/MM/yyyy (ex: 31/01/2026) ou deixe vazio.");
        }
    }

    public static TEnum ReadEnum<TEnum>(string label, TEnum defaultValue) where TEnum : struct, Enum
    {
        while (true)
        {
            Console.Write(label);
            var s = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(s)) return defaultValue;

            if (Enum.TryParse<TEnum>(s, true, out var e) && Enum.IsDefined(typeof(TEnum), e))
                return e;

            Console.WriteLine($"Valor inválido. Opções: {string.Join(", ", Enum.GetNames(typeof(TEnum)))}");
        }
    }

    public static TEnum? ReadOptionalEnum<TEnum>(string label) where TEnum : struct, Enum
    {
        Console.Write(label);
        var s = (Console.ReadLine() ?? "").Trim();
        if (string.IsNullOrWhiteSpace(s)) return null;

        if (Enum.TryParse<TEnum>(s, true, out var e) && Enum.IsDefined(typeof(TEnum), e))
            return e;

        Console.WriteLine($"Valor inválido. Mantendo valor anterior.");
        return null;
    }
}
