using AutoMapper;
using Common.Events;
using Common.Interfaces;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using ReviewService.Data;
using ReviewService.Interfaces;
using ReviewService.Models;
using System;
using System.Threading.Tasks;

namespace ReviewService.Services
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        private readonly IMessageTracker _messageTracker;
        private readonly IMapper _mapper;
        private readonly ReviewDbContext _context;
        private readonly ILogger _logger;

        public SubscriberService(IMessageTracker messageTracker,
                                 IMapper mapper,
                                 ReviewDbContext context,
                                 ILogger<SubscriberService> logger)
        {
            _messageTracker = messageTracker;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }


        [CapSubscribe("browsingservice.series.created")]
        public async Task ReceiveSeriesCreated(SeriesCreatedEvent seriesEvent)
        {
            if (!await _messageTracker.HasProcessed(seriesEvent.EventId))
            {
                var series = _mapper.Map<Series>(seriesEvent);

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Series.Add(series);
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
                var episode = _mapper.Map<Episode>(episodeEvent);
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Episode.Add(episode);
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

        [CapSubscribe("identityservice.user.created")]
        public async Task ReceiveUserCreated(UserCreatedEvent userEvent)
        {
            if (!await _messageTracker.HasProcessed(userEvent.EventId))
            {
                var reviewer = _mapper.Map<Reviewer>(userEvent);
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        _context.Reviewer.Add(reviewer);
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
    }
}
