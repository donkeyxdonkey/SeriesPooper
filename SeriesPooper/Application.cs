using SeriesPooper.Enumerations;
using SeriesPooper.Interface;
using SeriesPooper.TestClass;
using System;
using System.Collections.Generic;
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
    private SerieLibrary _serieLibrary;

    public Application()
    {
        FileInfo config = new(Path.Combine(AppContext.BaseDirectory, CONFIG_FILE));

        _state = State.Idle;
        _serieLibrary = YamlParser.ParseConfig<SerieLibrary>(config);
    }

    public void Start()
    {
        _state = State.Running;
    }
}
