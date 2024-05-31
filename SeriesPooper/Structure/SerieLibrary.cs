using SeriesPooper.Application;
using SeriesPooper.Enumerations;
using SeriesPooper.Interface;

namespace SeriesPooper.TestClass;

internal class SerieLibrary : ISerieLibrary
{
    const int SERIE_PADDING = 25;
    const int EPISODE_PADDING = 67;

    const string LINE_SEPARATOR = "-------------------------------------------------------------------------------------------------------------";

    #region ----- AUTO PROPERTIES
    public List<Serie> Library { get; set; } = [];

    internal class Serie
    {
        public string? Name { get; set; }

        public List<Season> Seasons { get; set; } = [];

        public override string ToString()
        {
            int combinedEpisodes = Seasons
                .Sum(x => x.Episodes.Count);

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

    public void ListSeries()
    {
        if (Library is null)
            return;

        foreach (Serie serie in Library)
        {
            Console.WriteLine(serie.ToString());
        }
    }

    public void ListRecentlyWatched()
    {
        var recentEpisodes = Library
            .SelectMany(serie => serie.Seasons, (serie, season) => new { SerieName = serie.Name, Season = season })
            .SelectMany(s => s.Season.Episodes, (s, episode) => new { s.SerieName, Episode = episode })
            .Where(x => x.Episode.DateWatched.HasValue)
            .OrderByDescending(x => x.Episode.DateWatched)
            .Take(15)
            .ToList();

        Console.WriteLine(LINE_SEPARATOR);
        Console.WriteLine("   MOST RECENTLY WATCHED");
        Console.WriteLine($"    {"SERIE",-SERIE_PADDING} {"EPISODE",-EPISODE_PADDING} DATE");
        foreach (var item in recentEpisodes)
        {
            string formattedOutput = $"    {item.SerieName?.PadRight(SERIE_PADDING)} {item.Episode.Name?.PadRight(EPISODE_PADDING)} {item.Episode.DateWatched:yyyy-MM-dd}";
            Console.WriteLine(formattedOutput);
        }
        Console.WriteLine(LINE_SEPARATOR);
        Menu.DrawMenuItems([MenuItems.MENU, MenuItems.EXIT]);
    }
}
