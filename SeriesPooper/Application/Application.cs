using SeriesPooper.Enumerations;
using SeriesPooper.Interface;
using SeriesPooper.TestClass;
using SeriesPooper.Utility;
using System.Diagnostics;

namespace SeriesPooper.Application;

internal class Application : IApplication
{
    #region ----- CONSTANTS
    const string CONFIG_FILE = "series.yaml";
    #endregion

    #region ----- PROPERTIES
    public State State
    {
        get => _state;
    }
    #endregion

    #region ----- FIELDS
    private State _state;
    private readonly FileInfo _config;
    private readonly SerieLibrary _serieLibrary;
    #endregion

    #region ----- CONSTRUCTOR
    public Application()
    {
        _state = State.Idle;
        _config = new(Path.Combine(AppContext.BaseDirectory, CONFIG_FILE));
        _serieLibrary = YamlParser.ParseConfig<SerieLibrary>(_config);
    }
    #endregion

    #region ----- MAIN LOOP
    public void Start()
    {
        _state = State.Running;

        try
        {
            Menu.DrawSeparator();
            Menu.Draw();
            Menu.DrawSeparator();
            Menu.DrawEmpty();

            do
            {
                AwaitAction();
            } while (_state == State.Running);
        }
        catch (Exception ex)
        {
            StackFrame stackFrame = new(skipFrames: 1, needFileInfo: true);
            HandleException(ex, stackFrame.GetMethod()!.Name);
        }
    }

    private void AwaitAction()
    {
        _serieLibrary.ListRecentlyWatched();
        _serieLibrary.ListSeries();

    }

    private static void HandleException(Exception ex, string methodName)
    {
        Console.WriteLine($"Error in: {methodName}");
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }
    #endregion

    private void Save()
    {
        YamlParser.SaveConfig(_serieLibrary, _config);
    }
}
