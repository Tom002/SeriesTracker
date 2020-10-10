using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.Models;

namespace ProfileService.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
       : base(options)
        {
        }

        public UserDbContext()
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .Property(v => v.UserId)
                .ValueGeneratedNever();

            builder.Entity<ProcessedEvent>()
                .Property(p => p.EventId)
                .ValueGeneratedNever();
        }
    }
}
