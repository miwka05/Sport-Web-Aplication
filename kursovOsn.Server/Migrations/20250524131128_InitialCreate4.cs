using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kursovOsn.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stat_ID",
                table: "Team_Statistics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stat_ID",
                table: "Player_Statistics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Team_Statistics_Stat_ID",
                table: "Team_Statistics",
                column: "Stat_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Statistics_Stat_ID",
                table: "Player_Statistics",
                column: "Stat_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Statistics_Sport_Statistics_Stat_ID",
                table: "Player_Statistics",
                column: "Stat_ID",
                principalTable: "Sport_Statistics",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Statistics_Sport_Statistics_Stat_ID",
                table: "Team_Statistics",
                column: "Stat_ID",
                principalTable: "Sport_Statistics",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Statistics_Sport_Statistics_Stat_ID",
                table: "Player_Statistics");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_Statistics_Sport_Statistics_Stat_ID",
                table: "Team_Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Team_Statistics_Stat_ID",
                table: "Team_Statistics");

            migrationBuilder.DropIndex(
                name: "IX_Player_Statistics_Stat_ID",
                table: "Player_Statistics");

            migrationBuilder.DropColumn(
                name: "Stat_ID",
                table: "Team_Statistics");

            migrationBuilder.DropColumn(
                name: "Stat_ID",
                table: "Player_Statistics");
        }
    }
}
