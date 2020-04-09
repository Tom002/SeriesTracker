using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Helpers
{
    public class ArtistParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public enum SortBy
        {
            Alphabetical = 1,
            AlphabeticalDescending = 2
        }

        public enum ActorWriter
        {
            Actor = 1,
            Writer = 2,
            Both = 3,
            Any = 4
        }

        public SortBy Sort { get; set; } = SortBy.Alphabetical;

        public ActorWriter Occupation { get; set; } = ActorWriter.Actor;

        public string NameFilter { get; set; }
    }
}
