namespace SeriesPooper.Utility;

internal class ConsoleUtility
{
    public static int[] LineSeparators = [0, 0];

    public static void ClearLines(int endLine, int startLine = 0)
    {
        for (int i = startLine; i <= endLine; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(0, startLine);
    }

    public static void ClearContent()
    {
        if (LineSeparators[0] == LineSeparators[1])
            return;

        ClearLines(LineSeparators[1], LineSeparators[0]);
    }
}
