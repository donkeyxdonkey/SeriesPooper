using SeriesPooper;
using SeriesPooper.Interface;
using SeriesPooper.TestClass;

internal class Program
{
    private static void Main(string[] args)
    {
        IApplication app = new Application();
        app.Start();
    }
}