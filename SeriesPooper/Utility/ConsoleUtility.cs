namespace SeriesPooper.Utility;

internal class ConsoleUtility
{
    public static void ClearLines(int endLine, int startLine = 0)
    {
        for (int i = startLine; i <= endLine; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(0, startLine);
    }
}
