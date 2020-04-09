using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Helpers
{
    public class SeriesParams
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
            HighestRating = 1,
            LowestRating = 2,
            MostReviewed = 3,
            Alphabetical = 4,
            AlphabeticalDescending = 5,
        }

        public int CategoryId { get; set; }
        public SortBy Sort { get; set; } = SortBy.HighestRating;

        public string TitleFilter { get; set; }
    }
}
