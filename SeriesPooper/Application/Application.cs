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
    private ApplicationAction _action;
    private readonly FileInfo _config;
    private readonly SerieLibrary _serieLibrary;
    private readonly KeyListener _keyListener;
    #endregion

    #region ----- CONSTRUCTOR
    public Application()
    {
        _state = State.Idle;
        _config = new(Path.Combine(AppContext.BaseDirectory, CONFIG_FILE));
        _serieLibrary = YamlParser.ParseConfig<SerieLibrary>(_config);
        _keyListener = new();
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

            _action = ApplicationAction.ListRecent;

            do
            {
                AwaitAction();
                HandleAction();
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
        switch (_action)
        {
            case ApplicationAction.None:
                break;
            case ApplicationAction.ListRecent:
                _serieLibrary.ListRecentlyWatched();
                break;
            case ApplicationAction.ListSeries:
                _serieLibrary.ListSeries();
                break;
            case ApplicationAction.BrowseBack:
                break;
            case ApplicationAction.Exit:
                _state = State.Terminate;
                break;
            default:
                break;
        }
    }

    private void HandleAction()
    {
        ConsoleKey keyPressed = _keyListener.Listen();

        _action = _action switch
        {
            ApplicationAction.None => Menu.Act(keyPressed),
            ApplicationAction.ListRecent => Menu.Act(keyPressed),
            ApplicationAction.ListSeries => ApplicationAction.None, // TODO
            ApplicationAction.BrowseBack => ApplicationAction.None, // TODO
            ApplicationAction.Exit => ApplicationAction.None, // TODO
            _ => ApplicationAction.None
        };
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
