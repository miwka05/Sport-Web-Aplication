using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace kursovOsn.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sport_Statistics",
                columns: new[] { "ID", "DataType", "Sport_ID", "Stat_Name" },
                values: new object[,]
                {
                    { 1, "int", 7, "Голы" },
                    { 2, "int", 7, "Пасы" },
                    { 3, "int", 7, "Фолы" },
                    { 4, "int", 7, "Удары по воротам" },
                    { 5, "int", 7, "Удары в створ" },
                    { 6, "int", 7, "Офсайды" },
                    { 7, "int", 7, "Угловые" },
                    { 8, "int", 7, "Карточки (жёлтые)" },
                    { 9, "int", 7, "Карточки (красные)" },
                    { 10, "float", 7, "Владение мячом (%)" },
                    { 11, "float", 7, "Точные передачи (%)" },
                    { 12, "int", 8, "Очки" },
                    { 13, "int", 8, "Подборы" },
                    { 14, "int", 8, "Передачи" },
                    { 15, "int", 8, "Блок-шоты" },
                    { 16, "int", 8, "Перехваты" },
                    { 17, "int", 8, "Потери" },
                    { 18, "int", 8, "Фолы" },
                    { 19, "int", 8, "3-очковые попадания" },
                    { 20, "int", 8, "3-очковые попытки" },
                    { 21, "int", 8, "2-очковые попадания" },
                    { 22, "int", 8, "2-очковые попытки" },
                    { 23, "int", 8, "Штрафные попадания" },
                    { 24, "int", 8, "Штрафные попытки" },
                    { 25, "float", 8, "Эффективность (%)" },
                    { 26, "int", 9, "Эйсы" },
                    { 27, "int", 9, "Двойные ошибки" },
                    { 28, "int", 9, "Выигранные подачи" },
                    { 29, "float", 9, "Процент первой подачи" },
                    { 30, "int", 9, "Выигранные розыгрыши" },
                    { 31, "int", 9, "Брейк-поинты реализованы" },
                    { 32, "int", 9, "Брейк-поинты всего" },
                    { 33, "int", 9, "Ошибки (невынужденные)" },
                    { 34, "int", 9, "Вынужденные ошибки" },
                    { 35, "float", 9, "Процент выигранных очков" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Sport_Statistics",
                keyColumn: "ID",
                keyValue: 35);
        }
    }
}
