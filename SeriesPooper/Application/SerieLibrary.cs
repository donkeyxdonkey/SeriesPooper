using SeriesPooper.Data;
using SeriesPooper.Enumerations;
using SeriesPooper.Interface;
using SeriesPooper.Utility;

namespace SeriesPooper.Application;

internal class SerieLibrary : ISerieLibrary
{
    const int SERIE_PADDING = 25;
    const int EPISODE_PADDING = 50;
    const int SEASON_PADDING = 30;
    const int SEASON_ID_PADDING = 16;

    #region ----- AUTO PROPERTIES
    public List<Serie> Library { get; set; } = [];

    internal class Serie
    {
        // TODO: gör separata klasser när klar.
        public string? Name { get; set; }

        public List<Season> Seasons { get; set; } = [];

        public override string ToString()
        {
            foreach (Season season in Seasons)
            {
                int count = season.Episodes.Count;
            }

            int combinedEpisodes = Seasons
                .Sum(season => season.Episodes!.Count);

            return $"{Name} - Seasons {Seasons!.Count} - Episodes {combinedEpisodes}";
        }

        internal class Season
        {
            public int Id { get; set; }

            public List<Episode> Episodes { get; set; } = [];

            internal class Episode
            {
                public int Id { get; set; }

                public string? Name { get; set; }

                public bool Watched { get; set; }

                public DateTime? DateWatched { get; set; }
            }
        }
    }
    #endregion

    public void ListSeries(Action<IEnumerable<MenuItems>, int, ushort, ushort> callbackUpdateMenu)
    {
        if (Library is null)
            return;

        var seriesSummary = Library
            .Select(serie => new
            {
                SerieName = serie.Name,
                SeasonsCount = serie.Seasons.Count,
                EpisodesWatched = serie.Seasons.Sum(x => x.Episodes.Count(x => x.DateWatched.HasValue)),
                EpisodesTotal = serie.Seasons.Sum(x => x.Episodes.Count)
            })
            .OrderBy(x => x.SerieName)
            .ToList();

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("   SERIES");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"    {"SERIE",-SERIE_PADDING} {"SEASONS",-SEASON_PADDING} {"WATCHED",-8} TOTAL");
        Console.ForegroundColor = ConsoleColor.White;

        int index = 0;

        foreach (var item in seriesSummary)
        {
            if (index++ == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(" >> ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
                Console.Write("    ");

            Console.WriteLine($"{item.SerieName?.PadRight(SERIE_PADDING)} {item.SeasonsCount,-SEASON_PADDING} {item.EpisodesWatched,-8} {item.EpisodesTotal}");
        }

        ushort cursorLocation = (ushort)(ConsoleUtility.LineSeparators[0] + 2);
        callbackUpdateMenu?.Invoke(Enumerable.Repeat(MenuItems.SERIE, seriesSummary.Count), 0, cursorLocation, 0);
    }

    public void ListRecentlyWatched()
    {
        var recentEpisodes = Library
            .SelectMany(serie => serie.Seasons, (serie, season) => new { SerieName = serie.Name, Season = season })
            .SelectMany(s => s.Season.Episodes, (s, episode) => new { s.SerieName, s.Season.Id, Episode = episode })
            .Where(x => x.Episode.DateWatched.HasValue)
            .OrderByDescending(x => x.Episode.DateWatched)
            .Take(15)
            .ToList();

        if (ConsoleUtility.LineSeparators[0] == 0)                                                                      /* behövs bara sättas 1 gång. */
        {
            Console.WriteLine(ApplicationData.LINE_SEPARATOR);
            ConsoleUtility.LineSeparators[0] = Console.CursorTop;
        }

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine("   MOST RECENTLY WATCHED");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"    {"SERIE",-SERIE_PADDING} {"EPISODE",-EPISODE_PADDING} {"SEASON",-SEASON_ID_PADDING} DATE");
        Console.ForegroundColor = ConsoleColor.White;

        foreach (var item in recentEpisodes)
        {
            Console.WriteLine($"    {item.SerieName?.PadRight(SERIE_PADDING)} {item.Episode.Name?.PadRight(EPISODE_PADDING)} S{item.Id,-SEASON_ID_PADDING} {item.Episode.DateWatched:yyyy-MM-dd}");
        }
        ConsoleUtility.LineSeparators[1] = Console.CursorTop;
        Console.WriteLine(ApplicationData.LINE_SEPARATOR);
        Menu.DrawMenuItems([MenuItems.MENU, MenuItems.EXIT]);
    }
}
