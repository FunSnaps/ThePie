using Microsoft.EntityFrameworkCore.Migrations;

namespace ThePie.Data.Migrations
{
    public partial class ImageUp_PatchV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PosterId",
                table: "Comic",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comic_PosterId",
                table: "Comic",
                column: "PosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comic_Poster_PosterId",
                table: "Comic",
                column: "PosterId",
                principalTable: "Poster",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comic_Poster_PosterId",
                table: "Comic");

            migrationBuilder.DropIndex(
                name: "IX_Comic_PosterId",
                table: "Comic");

            migrationBuilder.DropColumn(
                name: "PosterId",
                table: "Comic");
        }
    }
}
