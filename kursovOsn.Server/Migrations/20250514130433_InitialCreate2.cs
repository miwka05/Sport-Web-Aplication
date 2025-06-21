using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kursovOsn.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator_ID",
                table: "Tournaments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator_ID",
                table: "Teams",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_Creator_ID",
                table: "Tournaments",
                column: "Creator_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Creator_ID",
                table: "Teams",
                column: "Creator_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_Creator_ID",
                table: "Teams",
                column: "Creator_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_AspNetUsers_Creator_ID",
                table: "Tournaments",
                column: "Creator_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_Creator_ID",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_AspNetUsers_Creator_ID",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_Creator_ID",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Teams_Creator_ID",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Creator_ID",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Creator_ID",
                table: "Teams");
        }
    }
}
