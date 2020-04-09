using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WatchingService.Migrations
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
                    SeriesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.EpisodeId);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "Viewer",
                columns: table => new
                {
                    ViewerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viewer", x => x.ViewerId);
                });

            migrationBuilder.CreateTable(
                name: "EpisodeWatched",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false),
                    ViewerId = table.Column<string>(nullable: false),
                    WatchingDate = table.Column<DateTime>(nullable: false),
                    IsInDiary = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodeWatched", x => new { x.ViewerId, x.EpisodeId });
                    table.ForeignKey(
                        name: "FK_EpisodeWatched_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "EpisodeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpisodeWatched_Viewer_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "Viewer",
                        principalColumn: "ViewerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesLiked",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    ViewerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesLiked", x => new { x.ViewerId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_SeriesLiked_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesLiked_Viewer_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "Viewer",
                        principalColumn: "ViewerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesWatched",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    ViewerId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesWatched", x => new { x.ViewerId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_SeriesWatched_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesWatched_Viewer_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "Viewer",
                        principalColumn: "ViewerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeWatched_EpisodeId",
                table: "EpisodeWatched",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesLiked_SeriesId",
                table: "SeriesLiked",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesWatched_SeriesId",
                table: "SeriesWatched",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpisodeWatched");

            migrationBuilder.DropTable(
                name: "SeriesLiked");

            migrationBuilder.DropTable(
                name: "SeriesWatched");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Viewer");
        }
    }
}
