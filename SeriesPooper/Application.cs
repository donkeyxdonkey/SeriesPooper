﻿using SeriesPooper.Enumerations;
using SeriesPooper.Interface;
using SeriesPooper.TestClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPooper;
internal class Application : IApplication
{
    const string CONFIG_FILE = "config.yaml";

    public State State
    {
        get => _state;
    }

    private State _state;
    private FileInfo _config;
    private SerieLibrary _serieLibrary;

    public Application()
    {
        _state = State.Idle;
        _config = new(Path.Combine(AppContext.BaseDirectory, CONFIG_FILE));
        _serieLibrary = YamlParser.ParseConfig<SerieLibrary>(_config);
    }

    public void Start()
    {
        _state = State.Running;

        try
        {
            do
            {

            } while (_state == State.Running);
        }
        catch (Exception ex)
        {
            StackFrame stackFrame = new(skipFrames: 1, needFileInfo: true);
            HandleException(ex, stackFrame.GetMethod()!.Name);
        }
    }

    private static void HandleException(Exception ex, string methodName)
    {
        Console.WriteLine($"Error in: {methodName}");
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
    }
}
