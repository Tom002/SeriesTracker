using BrowsingService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowsingService.Data
{
    public class BrowsingDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<SeriesReview> SeriesReview { get; set; }
        public DbSet<EpisodeReview> EpisodeReview { get; set; }
        public DbSet<SeriesWriter> SeriesWriter { get; set; }
        public DbSet<Category> Categories { get; set; }

        public BrowsingDbContext(DbContextOptions<BrowsingDbContext> options)
            : base(options)
        {
        }

        public BrowsingDbContext()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SeriesActor>()
                .HasKey(sa => new { sa.ArtistId, sa.SeriesId });
            builder.Entity<SeriesActor>()
                .HasOne(sa => sa.Artist)
                .WithMany(a => a.AppearedIn)
                .HasForeignKey(sa => sa.ArtistId);
            builder.Entity<SeriesActor>()
                .HasOne(sa => sa.Series)
                .WithMany(s => s.Cast)
                .HasForeignKey(sa => sa.SeriesId);

            builder.Entity<SeriesWriter>()
                .HasKey(sw => new { sw.ArtistId, sw.SeriesId });
            builder.Entity<SeriesWriter>()
                .HasOne(sw => sw.Artist)
                .WithMany(a => a.WriterOf)
                .HasForeignKey(sw => sw.ArtistId);
            builder.Entity<SeriesWriter>()
                .HasOne(sw => sw.Series)
                .WithMany(s => s.Writers)
                .HasForeignKey(sw => sw.SeriesId);

            builder.Entity<SeriesCategory>()
                .HasKey(sc => new { sc.CategoryId, sc.SeriesId });
            builder.Entity<SeriesCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.Series)
                .HasForeignKey(sc => sc.CategoryId);
            builder.Entity<SeriesCategory>()
                .HasOne(sc => sc.Series)
                .WithMany(s => s.Categories)
                .HasForeignKey(sc => sc.SeriesId);

            builder.Entity<SeriesReview>()
                .HasKey(review => new { review.ReviewerId, review.SeriesId });

            builder.Entity<EpisodeReview>()
                .HasKey(review => new { review.ReviewerId, review.EpisodeId });

            builder.Entity<Series>()
                .Property(s => s.SeriesId)
                .ValueGeneratedNever();

            builder.Entity<Episode>()
                .Property(e => e.EpisodeId)
                .ValueGeneratedNever();

            builder.Entity<Artist>()
                .Property(a => a.ArtistId)
                .ValueGeneratedNever();

            builder.Entity<Category>()
                .Property(c => c.CategoryId)
                .ValueGeneratedNever();

            

            base.OnModelCreating(builder);
        }
    }
}
