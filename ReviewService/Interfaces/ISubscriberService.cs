using Common.Events;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Interfaces
{
    public interface ISubscriberService
    {
        public Task ReceiveSeriesCreated(SeriesCreatedEvent seriesEvent);

        public Task ReceiveEpisodeCreated(EpisodeCreatedEvent episodeEvent);

        public Task ReceiveUserCreated(UserCreatedEvent userEvent);
    }
}
