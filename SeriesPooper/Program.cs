using SeriesPooper;
using SeriesPooper.Interface;

internal class Program
{
    private static void Main(string[] args)
    {
        IApplication app = new Application();
        app.Start();
    }
}