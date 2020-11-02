using AutoMapper;
using Common.Events;
using Common.Interfaces;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WatchingService.Data;
using WatchingService.Helpers;
using WatchingService.Interfaces;
using WatchingService.Models;
using WatchingService.Dto.Email;

namespace WatchingService.Services
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        private readonly IMessageTracker _messageTracker;
        private readonly IMapper _mapper;
        private readonly WatchingDbContext _context;
        private readonly ILogger _logger;
        private readonly ICapPublisher _capBus;
        private readonly IEmailSender _emailSender;

        public SubscriberService(IMessageTracker messageTracker,
                                 IMapper mapper,
                                 WatchingDbContext context,
                                 ILogger<SubscriberService> logger,
                                 ICapPublisher capBuS,
                                 IEmailSender emailSender)
        {
            _messageTracker = messageTracker;
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _capBus = capBuS;
            _emailSender = emailSender;
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

                        // Email értesítés küldése azoknak a felhasználóknak akik nézik ezt a sorozatot,
                        // hogy új rész jött ki

                        var series = await _context.Series
                            .FirstAsync(s => s.SeriesId == episodeCreated.SeriesId);

                        var viewers = await _context.Series
                            .Where(s => s.SeriesId == episodeCreated.SeriesId)
                            .Select(s => s.SeriesWatched.Select(sw => sw.Viewer))
                            .FirstAsync();

                        foreach (var viewer in viewers)
                        {
                            //var notificationMessage = new Message(
                            //    //new string[] { viewer.Email },
                            //    new string[] { "tamas.princz3@gmail.com" },
                            //    $"New {series.Title} episode released",
                            //    $"Episode title:{episodeCreated.EpisodeTitle}" +
                            //    $"Season:{episodeCreated.Season} Episode:{episodeCreated.EpisodeNumber}"
                            //);
                            //await _emailSender.SendEmailAsync(notificationMessage);
                        }

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
