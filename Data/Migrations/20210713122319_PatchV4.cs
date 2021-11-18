using Microsoft.EntityFrameworkCore.Migrations;

namespace ThePie.Data.Migrations
{
    public partial class PatchV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComicImage",
                table: "Comic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComicImage",
                table: "Comic",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
