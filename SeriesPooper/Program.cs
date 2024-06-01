using SeriesPooper.Application;
using SeriesPooper.Utility;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WindowHeight = Console.LargestWindowHeight - 10;

        if (OperatingSystem.IsWindows())
            Console.SetWindowPosition(0, 0);

        Console.Clear();
        Console.SetCursorPosition(0, 0);

        using (DeferCursor defer = DeferCursor.Defer())
        {
            new Application(args).Start();
        }

        ConsoleUtility.ClearLines(Console.CursorTop);
        Console.SetCursorPosition(0, 0);
    }
}