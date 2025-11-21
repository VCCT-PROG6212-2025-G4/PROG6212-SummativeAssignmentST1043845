using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS_Web_App.Migrations
{
    /// <inheritdoc />
    public partial class AddLecturerPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Lecturers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 1,
                column: "PasswordHash",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 2,
                column: "PasswordHash",
                value: null);

            migrationBuilder.UpdateData(
                table: "Lecturers",
                keyColumn: "LecturerId",
                keyValue: 3,
                column: "PasswordHash",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Lecturers");
        }
    }
}
