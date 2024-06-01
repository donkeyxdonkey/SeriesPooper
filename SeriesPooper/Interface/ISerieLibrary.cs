using SeriesPooper.Enumerations;

namespace SeriesPooper.Interface;
internal interface ISerieLibrary
{
    public void ListSeries(Action<IEnumerable<MenuItems>, int, ushort, ushort> callbackUpdateMenu);

    public void ListRecentlyWatched();
}
