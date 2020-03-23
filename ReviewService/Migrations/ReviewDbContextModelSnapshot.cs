﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReviewService.Data;

namespace ReviewService.Migrations
{
    [DbContext(typeof(ReviewDbContext))]
    partial class ReviewDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReviewService.Models.Episode", b =>
                {
                    b.Property<int>("EpisodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.HasKey("EpisodeId");

                    b.ToTable("Episode");
                });

            modelBuilder.Entity("ReviewService.Models.EpisodeReview", b =>
                {
                    b.Property<int>("EpisodeId")
                        .HasColumnType("int");

                    b.Property<string>("ReviewerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("EpisodeId", "ReviewerId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("EpisodeReview");
                });

            modelBuilder.Entity("ReviewService.Models.Reviewer", b =>
                {
                    b.Property<string>("ReviewerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReviewerId");

                    b.ToTable("Reviewer");
                });

            modelBuilder.Entity("ReviewService.Models.Series", b =>
                {
                    b.Property<int>("SeriesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("SeriesId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("ReviewService.Models.SeriesReview", b =>
                {
                    b.Property<int>("SeriesId")
                        .HasColumnType("int");

                    b.Property<string>("ReviewerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReviewText")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("ReviewTitle")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("SeriesId", "ReviewerId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("SeriesReview");
                });

            modelBuilder.Entity("ReviewService.Models.EpisodeReview", b =>
                {
                    b.HasOne("ReviewService.Models.Episode", "Episode")
                        .WithMany("Reviews")
                        .HasForeignKey("EpisodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReviewService.Models.Reviewer", "Reviewer")
                        .WithMany("EpisodeReviews")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReviewService.Models.SeriesReview", b =>
                {
                    b.HasOne("ReviewService.Models.Reviewer", "Reviewer")
                        .WithMany("SeriesReviews")
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ReviewService.Models.Series", "Series")
                        .WithMany("Reviews")
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
