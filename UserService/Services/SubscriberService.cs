using AutoMapper;
using Common.Events;
using Common.Interfaces;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using ProfileService.Data;
using ProfileService.Interfaces;
using ProfileService.Models;
using System;
using System.Threading.Tasks;

namespace ProfileService.Services
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        private readonly IMessageTracker _messageTracker;
        private readonly IMapper _mapper;
        private readonly UserDbContext _context;
        private readonly ILogger _logger;

        public SubscriberService(IMessageTracker messageTracker,
                                 IMapper mapper,
                                 UserDbContext context,
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
            if(!await _messageTracker.HasProcessed(userEvent.EventId))
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var user = _mapper.Map<User>(userEvent);
                        _context.Users.Add(user);
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
