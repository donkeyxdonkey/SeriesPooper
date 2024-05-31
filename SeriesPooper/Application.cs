using SeriesPooper.Enumerations;
using SeriesPooper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPooper;
internal class Application : IApplication
{
    public State State
    {
        get => _state;
    }

    private State _state;

    public Application()
    {
        _state = State.Idle;
    }

    public void Start()
    {
        _state = State.Running;
    }
}
