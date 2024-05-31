using SeriesPooper.Interface;

namespace SeriesPooper.TestClass;

internal class SerieLibrary : ISerieLibrary
{
    #region ----- AUTO PROPERTIES
    public List<Serie>? Library { get; set; }

    internal class Serie
    {
        public string? Name { get; set; }

        public List<Season>? Seasons { get; set; }

        internal class Season
        {
            public int Id { get; set; }

            public List<Episode>? Episodes { get; set; }

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
}
