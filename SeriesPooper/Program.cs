using SeriesPooper;
using SeriesPooper.TestClass;

internal class Program
{
    private static void Main(string[] args)
    {
        FileInfo config = new(Path.Combine(AppContext.BaseDirectory, "test.yaml"));

        SerieLibrary testo = YamlParser.ParseConfig<SerieLibrary>(config);
    }
}