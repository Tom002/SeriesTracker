using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrowsingService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    DeathDate = table.Column<DateTime>(nullable: true),
                    City = table.Column<string>(maxLength: 200, nullable: true),
                    About = table.Column<string>(maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    CoverImageUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    StartYear = table.Column<int>(nullable: true),
                    EndYear = table.Column<int>(nullable: true),
                    LastAirDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesId);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false),
                    SeriesId = table.Column<int>(nullable: false),
                    EpisodeTitle = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1500, nullable: true),
                    Season = table.Column<int>(nullable: false),
                    EpisodeNumber = table.Column<int>(nullable: false),
                    LengthInMinutes = table.Column<int>(nullable: false),
                    Release = table.Column<DateTime>(nullable: true),
                    IsReleased = table.Column<bool>(nullable: false),
                    CoverImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.EpisodeId);
                    table.ForeignKey(
                        name: "FK_Episode_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesActor",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    ArtistId = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 100, nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesActor", x => new { x.ArtistId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_SeriesActor_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesActor_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false),
                    SeriesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesCategory", x => new { x.CategoryId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_SeriesCategory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesCategory_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesReview",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    ReviewerId = table.Column<string>(nullable: false),
                    ReviewTitle = table.Column<string>(maxLength: 200, nullable: true),
                    ReviewDate = table.Column<DateTime>(nullable: false),
                    ReviewText = table.Column<string>(maxLength: 400, nullable: true),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesReview", x => new { x.ReviewerId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_SeriesReview_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeriesWriter",
                columns: table => new
                {
                    SeriesId = table.Column<int>(nullable: false),
                    ArtistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeriesWriter", x => new { x.ArtistId, x.SeriesId });
                    table.ForeignKey(
                        name: "FK_SeriesWriter_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeriesWriter_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_EpisodeReview", x => new { x.ReviewerId, x.EpisodeId });
                    table.ForeignKey(
                        name: "FK_EpisodeReview_Episode_EpisodeId",
                        column: x => x.EpisodeId,
                        principalTable: "Episode",
                        principalColumn: "EpisodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Episode_SeriesId",
                table: "Episode",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_EpisodeReview_EpisodeId",
                table: "EpisodeReview",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesActor_SeriesId",
                table: "SeriesActor",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesCategory_SeriesId",
                table: "SeriesCategory",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesReview_SeriesId",
                table: "SeriesReview",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_SeriesWriter_SeriesId",
                table: "SeriesWriter",
                column: "SeriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpisodeReview");

            migrationBuilder.DropTable(
                name: "SeriesActor");

            migrationBuilder.DropTable(
                name: "SeriesCategory");

            migrationBuilder.DropTable(
                name: "SeriesReview");

            migrationBuilder.DropTable(
                name: "SeriesWriter");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Series");
        }
    }
}
