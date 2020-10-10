using Common.Events;
using System.Threading.Tasks;

namespace WatchingService.Interfaces
{
    public interface ISubscriberService
    {
        public Task ReceiveUserCreated(UserCreatedEvent userEvent);

        public Task ReceiveSeriesCreated(SeriesCreatedEvent seriesEvent);

        public Task ReceiveEpisodeCreated(EpisodeCreatedEvent episodeEvent);
    }
}
