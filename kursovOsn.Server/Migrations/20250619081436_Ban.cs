using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kursovOsn.Server.Migrations
{
    /// <inheritdoc />
    public partial class Ban : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ban",
                table: "Tournaments",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reasonBan",
                table: "Tournaments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ban",
                table: "Teams",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reasonBan",
                table: "Teams",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ban",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "reasonBan",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "ban",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "reasonBan",
                table: "Teams");
        }
    }
}
