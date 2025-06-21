using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kursovOsn.Server.Migrations
{
    /// <inheritdoc />
    public partial class Ban2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "admin",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ban",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reasonBan",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "admin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ban",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "reasonBan",
                table: "AspNetUsers");
        }
    }
}
