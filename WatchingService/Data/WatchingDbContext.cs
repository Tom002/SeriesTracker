using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchingService.Models;

namespace WatchingService.Data
{
    public class WatchingDbContext : DbContext
    {
        public DbSet<EpisodeWatched> EpisodeWatched { get; set; }

        public DbSet<SeriesWatched> SeriesWatched { get; set; }

        public DbSet<SeriesLiked> SeriesLiked { get; set; }

        public DbSet<Viewer> Viewer { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<Episode> Episode { get; set; }

        public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

        public WatchingDbContext(DbContextOptions<WatchingDbContext> options)
       : base(options)
        {
        }

        public WatchingDbContext()
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Viewer>()
                .Property(v => v.ViewerId)
                .ValueGeneratedNever();

            builder.Entity<Series>()
                .Property(s => s.SeriesId)
                .ValueGeneratedNever();

            builder.Entity<Episode>()
                .Property(e => e.EpisodeId)
                .ValueGeneratedNever();

            builder.Entity<EpisodeWatched>()
               .HasKey(ew => new { ew.ViewerId, ew.EpisodeId });
            builder.Entity<EpisodeWatched>()
                .HasOne(ew => ew.Episode)
                .WithMany(e => e.EpisodesWatched)
                .HasForeignKey(ew => ew.EpisodeId);
            builder.Entity<EpisodeWatched>()
                .HasOne(ew => ew.Viewer)
                .WithMany(us => us.EpisodesWatched)
                .HasForeignKey(ew => ew.ViewerId);

            builder.Entity<SeriesLiked>()
                .HasKey(sl => new { sl.ViewerId, sl.SeriesId });
            builder.Entity<SeriesLiked>()
                .HasOne(sl => sl.Series)
                .WithMany(s => s.SeriesLiked)
                .HasForeignKey(sl => sl.SeriesId);
            builder.Entity<SeriesLiked>()
                .HasOne(sl => sl.Viewer)
                .WithMany(us => us.SeriesLiked)
                .HasForeignKey(sl => sl.ViewerId);

            builder.Entity<SeriesWatched>()
                .HasKey(sw => new { sw.ViewerId, sw.SeriesId });
            builder.Entity<SeriesWatched>()
                .HasOne(sw => sw.Series)
                .WithMany(s => s.SeriesWatched)
                .HasForeignKey(sw => sw.SeriesId);
            builder.Entity<SeriesWatched>()
                .HasOne(sw => sw.Viewer)
                .WithMany(us => us.SeriesWatched)
                .HasForeignKey(sw => sw.ViewerId);

            builder.Entity<ProcessedEvent>()
                .Property(p => p.EventId)
                .ValueGeneratedNever();
        }
    }
}
