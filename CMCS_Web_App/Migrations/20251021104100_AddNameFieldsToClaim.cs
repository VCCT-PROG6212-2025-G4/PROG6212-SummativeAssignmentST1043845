using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS_Web_App.Migrations
{
    /// <inheritdoc />
    public partial class AddNameFieldsToClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims");

            migrationBuilder.AlterColumn<int>(
                name: "LecturerId",
                table: "Claims",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Claims",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Claims",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Claims");

            migrationBuilder.AlterColumn<int>(
                name: "LecturerId",
                table: "Claims",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
