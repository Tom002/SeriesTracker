using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Helpers
{
    public class SeriesFilter
    {
        public enum SortBy
        {
            HighestRating = 1,
            LowestRating = 2,
            MostReviewed = 3
        }

        public int CategoryId { get; set; }
        public SortBy Sort { get; set; } = SortBy.HighestRating;

        public string TitleFilter { get; set; }
    }
}
