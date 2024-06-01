namespace SeriesPooper.Utility;

internal class DeferCursor : IDisposable
{
    private DeferCursor()
    {
        Console.CursorVisible = false;
    }

    public static DeferCursor Defer() => new();

    public void Dispose()
    {
        Console.CursorVisible = true;
        GC.SuppressFinalize(this);
    }
}
