using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesPooper.TestClass;

internal class SerieLibrary
{
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
}
