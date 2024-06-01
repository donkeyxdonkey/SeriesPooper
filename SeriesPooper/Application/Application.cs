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
    public Application(string[] args)
    {
        _state = State.Idle;
        _config = new(Path.Combine(AppContext.BaseDirectory, CONFIG_FILE));
        ParseArgs(args);
        _serieLibrary = YamlParser.ParseConfig<SerieLibrary>(_config);
        _keyListener = new();
    }

    private void ParseArgs(string[] args)
    {
        if (args.Length == 0)
            return;

        Console.WriteLine($"BEGIN Reading args{Environment.NewLine}");

        foreach (string arg in args.Select(arg => arg.ToLower()).ToArray())
        {
            switch (arg)
            {
                case "--backup":
                    DirectoryInfo backupDirectory = new(Path.Combine(AppContext.BaseDirectory, "Backup"));
                    backupDirectory.Create();
                    FileInfo backup = new(Path.Combine(backupDirectory.FullName, $"{DateTime.Now:yyyy-MM-dd}_{CONFIG_FILE}"));
                    File.Copy(_config.FullName, backup.FullName, overwrite: true);
                    Console.WriteLine($"Backup performed: {backup.FullName}{Environment.NewLine}");
                    break;
                case "--enable-logging":
                    DirectoryInfo logsDirectory = new(Path.Combine(AppContext.BaseDirectory, "Logs"));
                    logsDirectory.Create();
                    // TODO
                    break;
                case "--log-file":
                    // TODO
                    break;
                default:
                    Console.WriteLine($"Unknown argument: {arg}");
                    return;
            }
        }

        Console.WriteLine("END Reading args");
        Console.ReadLine();
        Console.Clear();
        Console.SetCursorPosition(0, 0);
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

            while (_state == State.Running)
            {
                AwaitAction();
                HandleAction();
            }
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
        if (_action == ApplicationAction.Exit)
            return;

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
