using SeriesPooper.Enumerations;

namespace SeriesPooper.Interface;
internal interface IApplication
{
    public void Start();

    public State State { get; }
}
