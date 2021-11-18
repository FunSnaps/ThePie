using Microsoft.EntityFrameworkCore.Migrations;

namespace ThePie.Data.Migrations
{
    public partial class PatchV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Author");

            migrationBuilder.AddColumn<int>(
                name: "AuthorSex",
                table: "Author",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorSex",
                table: "Author");

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Author",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }
    }
}
