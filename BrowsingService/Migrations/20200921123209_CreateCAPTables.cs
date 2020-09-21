using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace BrowsingService.Migrations
{
    public partial class CreateCAPTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createCAPTablesScript = Path.Combine(@"/app", @"Migrations/Scripts/CreateCAPTables.sql");
            migrationBuilder.Sql(File.ReadAllText(createCAPTablesScript));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var deleteCAPTablesScript = Path.Combine(@"/app", @"Migrations/Scripts/DropCAPTables.sql");
            migrationBuilder.Sql(File.ReadAllText(deleteCAPTablesScript));
        }
    }
}
