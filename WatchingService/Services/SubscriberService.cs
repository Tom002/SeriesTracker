using AutoMapper;
using Common.Events;
using Common.Interfaces;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WatchingService.Data;
using WatchingService.Interfaces;
using WatchingService.Models;

namespace WatchingService.Services
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        private readonly IMessageTracker _messageTracker;
        private readonly IMapper _mapper;
        private readonly WatchingDbContext _context;
        private readonly ILogger _logger;

        public SubscriberService(IMessageTracker messageTracker,
                                 IMapper mapper,
                                 WatchingDbContext context,
                                 ILogger<SubscriberService> logger)
        {
            _messageTracker = messageTracker;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        [CapSubscribe("identityservice.user.created")]
        public async Task ReceiveUserCreated(UserCreatedEvent userEvent)
        {
            if (!await _messageTracker.HasProcessed(userEvent.EventId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var viewerCreated = _mapper.Map<Viewer>(userEvent);
                        _context.Viewer.Add(viewerCreated);
                        await _context.SaveChangesAsync();
                        await _messageTracker.MarkAsProcessed(userEvent.EventId);
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(e, $"Unexpected error while processing event of type {nameof(UserCreatedEvent)}" +
                            $" EventId: {userEvent.EventId} UserId:{userEvent.UserId}");
                    }
                }
            }
        }

        [CapSubscribe("browsingservice.series.created")]
        public async Task ReceiveSeriesCreated(SeriesCreatedEvent seriesEvent)
        {
            if (!await _messageTracker.HasProcessed(seriesEvent.EventId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var seriesCreated = _mapper.Map<Series>(seriesEvent);
                        _context.Series.Add(seriesCreated);
                        await _context.SaveChangesAsync();
                        await _messageTracker.MarkAsProcessed(seriesEvent.EventId);
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(e, $"Unexpected error while processing event of type {nameof(SeriesCreatedEvent)}" +
                            $" EventId: {seriesEvent.EventId} SeriesId:{seriesEvent.SeriesId}");
                    }
                }
            }
        }

        [CapSubscribe("browsingservice.episode.created")]
        public async Task ReceiveEpisodeCreated(EpisodeCreatedEvent episodeEvent)
        {
            if (!await _messageTracker.HasProcessed(episodeEvent.EventId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var episodeCreated = _mapper.Map<Episode>(episodeEvent);
                        _context.Episode.Add(episodeCreated);
                        await _context.SaveChangesAsync();
                        await _messageTracker.MarkAsProcessed(episodeEvent.EventId);
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(e, $"Unexpected error while processing event of type {nameof(EpisodeCreatedEvent)}" +
                            $" EventId: {episodeEvent.EventId} EpisodeId:{episodeEvent.EpisodeId}");
                    }
                }
            }
        }
    }
}
