using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReviewService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false),
                    SeriesId = table.Column<int>(nullable: false),
                    EpisodeTitle = table.Column<string>(maxLength: 200, nullable: true),
                    Season = table.Column<int>(nullable: false),
                    EpisodeNumber = table.Column<int>(nullable: false),
                    Release = table.Column<DateTime>(nullable: true),
                    CoverImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.EpisodeId);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedEvents",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedEvents", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Reviewer",
                columns: table => new
                {
                    ReviewerId = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviewer", x => x.ReviewerId);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    CoverImageUrl = table.Column<string>(nullable: true),
                    StartYear = table.Column<int>(nullable: false),
                    EndYear = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "EpisodeReview",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false),
                    ReviewerId = table.Column<string>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeReview", x => new { x.EpisodeId, x.ReviewerId });
                    table.ForeignKey(
                        name: "FK_EpisodeReview_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "EpisodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpisodeReview_Reviewer_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Reviewer",
                        principalColumn: "ReviewerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesReview",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    ReviewerId = table.Column<string>(nullable: false),
                    ReviewTitle = table.Column<string>(maxLength: 50, nullable: true),
                    ReviewDate = table.Column<DateTime>(nullable: false),
                    ReviewText = table.Column<string>(maxLength: 400, nullable: true),
                    Rating = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesReview", x => new { x.SeriesId, x.ReviewerId });
                    table.ForeignKey(
                        name: "FK_SeriesReview_Reviewer_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "Reviewer",
                        principalColumn: "ReviewerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesReview_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeReview_ReviewerId",
                table: "EpisodeReview",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesReview_ReviewerId",
                table: "SeriesReview",
                column: "ReviewerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpisodeReview");

            migrationBuilder.DropTable(
                name: "ProcessedEvents");

            migrationBuilder.DropTable(
                name: "SeriesReview");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Reviewer");

            migrationBuilder.DropTable(
                name: "Series");
        }
    }
}
