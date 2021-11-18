using Microsoft.EntityFrameworkCore.Migrations;

namespace ThePie.Data.Migrations
{
    public partial class ImageUpPatchV4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PosterId",
                table: "Author",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Author_PosterId",
                table: "Author",
                column: "PosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Poster_PosterId",
                table: "Author",
                column: "PosterId",
                principalTable: "Poster",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Poster_PosterId",
                table: "Author");

            migrationBuilder.DropIndex(
                name: "IX_Author_PosterId",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "PosterId",
                table: "Author");
        }
    }
}
