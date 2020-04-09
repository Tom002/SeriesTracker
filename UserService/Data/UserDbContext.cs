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

        public DbSet<Series> Series { get; set; }

        public DbSet<Episode> Episodes { get; set; }

        public DbSet<SeriesWatched> SeriesWatched { get; set; }

        public DbSet<EpisodeDiary> DiaryEpisodes { get; set; }

        public DbSet<SeriesLiked> SeriesLiked{ get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
       : base(options)
        {
        }

        public UserDbContext()
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EpisodeDiary>()
               .HasKey(ew => new { ew.UserId, ew.EpisodeId });
            builder.Entity<EpisodeDiary>()
                .HasOne(ew => ew.Episode)
                .WithMany(e => e.UserDiaries)
                .HasForeignKey(ew => ew.EpisodeId);
            builder.Entity<EpisodeDiary>()
                .HasOne(ew => ew.User)
                .WithMany(us => us.EpisodeDiary)
                .HasForeignKey(ew => ew.UserId);

            builder.Entity<SeriesLiked>()
                .HasKey(sl => new { sl.UserId, sl.SeriesId });
            builder.Entity<SeriesLiked>()
                .HasOne(sl => sl.Series)
                .WithMany(s => s.SeriesLiked)
                .HasForeignKey(sl => sl.SeriesId);
            builder.Entity<SeriesLiked>()
                .HasOne(sl => sl.User)
                .WithMany(us => us.SeriesLiked)
                .HasForeignKey(sl => sl.UserId);

            builder.Entity<SeriesWatched>()
                .HasKey(sw => new { sw.UserId, sw.SeriesId });
            builder.Entity<SeriesWatched>()
                .HasOne(sw => sw.Series)
                .WithMany(s => s.SeriesWatched)
                .HasForeignKey(sw => sw.SeriesId);
            builder.Entity<SeriesWatched>()
                .HasOne(sw => sw.User)
                .WithMany(us => us.SeriesWatched)
                .HasForeignKey(sw => sw.UserId);

            builder.Entity<SeriesFollowed>()
                .HasKey(sf => new { sf.UserId, sf.SeriesId });
            builder.Entity<SeriesFollowed>()
                .HasOne(sf => sf.Series)
                .WithMany(s => s.SeriesFollowed)
                .HasForeignKey(sf => sf.SeriesId);
            builder.Entity<SeriesFollowed>()
                .HasOne(sf => sf.User)
                .WithMany(us => us.SeriesFollowed)
                .HasForeignKey(sf => sf.UserId);

            builder.Entity<EpisodeCalendar>()
                .HasKey(ec => new { ec.EpisodeId, ec.UserId });
            builder.Entity<EpisodeCalendar>()
                .HasOne(ec => ec.User)
                .WithMany(us => us.EpisodeCalendar)
                .HasForeignKey(ec => ec.UserId);
        }
    }
}
