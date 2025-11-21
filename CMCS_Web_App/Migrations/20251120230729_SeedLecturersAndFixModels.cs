using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMCS_Web_App.Migrations
{
    /// <inheritdoc />
    public partial class SeedLecturersAndFixModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "Claims",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSubmitted",
                table: "Claims",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "Lecturers",
                columns: new[] { "LecturerId", "Department", "Email", "FirstName", "HRId", "LastName", "RatePerHour" },
                values: new object[,]
                {
                    { 1, "Mathematics", "Lofentse13@CMCSLEC.com", "Lofentse", null, "Moagi", 200.00m },
                    { 2, "Computer Science", "Karabo28@CMCSLEC.com", "Karabo", null, "Kgoebane", 220.00m },
                    { 3, "Physics", "Claudia06@CMCSLEC.com", "Claudia", null, "Brander", 210.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "HourlyRate",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSubmitted",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }
    }
}
