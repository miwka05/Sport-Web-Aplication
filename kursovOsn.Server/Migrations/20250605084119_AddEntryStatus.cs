using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kursovOsn.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tournament_Entries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tournament_Entries");
        }
    }
}
