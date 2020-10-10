﻿using AutoMapper;
using BrowsingService.Data;
using BrowsingService.Interfaces;
using BrowsingService.Models;
using Common.Events;
using Common.Interfaces;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BrowsingService.Services
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        private readonly IMessageTracker _messageTracker;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly BrowsingDbContext _context;

        public SubscriberService(IMessageTracker messageTracker,
                                 IMapper mapper,
                                 ILogger<SubscriberService> logger,
                                 BrowsingDbContext context)
        {
            _messageTracker = messageTracker;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        [CapSubscribe("reviewService.seriesReview.created")]
        public async Task ReceiveSeriesReviewCreated(SeriesReviewCreatedEvent reviewEvent)
        {
            if (!await _messageTracker.HasProcessed(reviewEvent.EventId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var review = _mapper.Map<SeriesReview>(reviewEvent);
                        _context.SeriesReview.Add(review);
                        await _context.SaveChangesAsync();
                        await _messageTracker.MarkAsProcessed(reviewEvent.EventId);
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(e, $"Unexpected error while processing event of type {nameof(SeriesReviewCreatedEvent)}" +
                            $" EventId: {reviewEvent.EventId} SeriesId:{reviewEvent.SeriesId} Reviewerd:{reviewEvent.ReviewerId}");
                    }
                }
            }
        }

        [CapSubscribe("reviewService.seriesReview.updated")]
        public async Task ReceiveSeriesReviewUpdated(SeriesReviewUpdatedEvent reviewEvent)
        {
            if (!await _messageTracker.HasProcessed(reviewEvent.EventId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var review = await _context.SeriesReview
                            .FirstAsync(r => r.ReviewerId == reviewEvent.ReviewerId &&
                                             r.SeriesId == reviewEvent.SeriesId);
                        _mapper.Map(reviewEvent, review);
                        await _context.SaveChangesAsync();
                        await _messageTracker.MarkAsProcessed(reviewEvent.EventId);
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)                                                 
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(e, $"Unexpected error while processing event of type {nameof(SeriesReviewUpdatedEvent)}" +
                            $" EventId: {reviewEvent.EventId} SeriesId:{reviewEvent.SeriesId} Reviewerd:{reviewEvent.ReviewerId}");
                    }
                }
            }
        }

        [CapSubscribe("reviewService.seriesReview.deleted")]
        public async Task ReceiveSeriesReviewDeleted(SeriesReviewUpdatedEvent reviewEvent)
        {
            if (!await _messageTracker.HasProcessed(reviewEvent.EventId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var review = await _context.SeriesReview
                            .FirstAsync(r => r.ReviewerId == reviewEvent.ReviewerId &&
                                             r.SeriesId == reviewEvent.SeriesId);
                        _mapper.Map(reviewEvent, review);
                        await _context.SaveChangesAsync();
                        await _messageTracker.MarkAsProcessed(reviewEvent.EventId);
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(e, $"Unexpected error while processing event of type {nameof(SeriesReviewUpdatedEvent)}" +
                            $" EventId: {reviewEvent.EventId} SeriesId:{reviewEvent.SeriesId} Reviewerd:{reviewEvent.ReviewerId}");
                    }
                }
            }
        }
    }
}
