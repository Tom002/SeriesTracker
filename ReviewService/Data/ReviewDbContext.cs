using Microsoft.EntityFrameworkCore;
using ReviewService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewService.Data
{
    public class ReviewDbContext : DbContext
    {
        public DbSet<EpisodeReview> EpisodeReview { get; set; }

        public DbSet<SeriesReview> SeriesReview { get; set; }

        public DbSet<Reviewer> Reviewer { get; set; }

        public DbSet<Series> Series { get; set; }

        public DbSet<Episode> Episode { get; set; }

        public ReviewDbContext(DbContextOptions<ReviewDbContext> options)
       : base(options)
        {
        }

        public ReviewDbContext()
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EpisodeReview>()
               .HasKey(review => new { review.EpisodeId, review.ReviewerId });
            builder.Entity<EpisodeReview>()
                .HasOne(review => review.Episode)
                .WithMany(episode => episode.Reviews)
                .HasForeignKey(review => review.EpisodeId);
            builder.Entity<EpisodeReview>()
                .HasOne(review => review.Reviewer)
                .WithMany(reviewer => reviewer.EpisodeReviews)
                .HasForeignKey(review => review.ReviewerId);

            builder.Entity<SeriesReview>()
                .HasKey(review => new { review.SeriesId, review.ReviewerId });
            builder.Entity<SeriesReview>()
                .HasOne(review => review.Series)
                .WithMany(series => series.Reviews)
                .HasForeignKey(review => review.SeriesId);
            builder.Entity<SeriesReview>()
                .HasOne(review => review.Reviewer)
                .WithMany(reviewer => reviewer.SeriesReviews)
                .HasForeignKey(review => review.ReviewerId);
        }
    }
}
