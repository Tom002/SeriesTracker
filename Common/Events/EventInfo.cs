
namespace Common.Events
{
    public class EventInfo
    {
        public const string SeriesReviewCreated = "reviewService.seriesReview.created";
        public const string SeriesReviewUpdated = "reviewService.seriesReview.updated";
        public const string EpisodeReviewCreated = "reviewService.episodeReview.created";
        public const string EpisodeReviewUpdated = "reviewService.episodeReview.updated";

        public const string SeriesWatchedCreated = "watchingService.seriesWatched.created";
        public const string SeriesWatchedDeleted = "watchingService.seriesWatched.deleted";
        public const string SeriesLikedCreated = "watchingService.seriesWatched.created";
        public const string SeriesLikedDeleted = "watchingService.seriesWatched.deleted";
        public const string EpisodeWatchedCreated = "watchingService.episodeWatched.created";
        public const string EpisodeWatchedDeleted = "watchingService.episodeWatched.deleted";

        public const string UserCreated = "identityservice.user.created";
    }
}
