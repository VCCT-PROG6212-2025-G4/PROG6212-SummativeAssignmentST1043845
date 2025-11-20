using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMCS_Web_App.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "HRId",
                table: "Lecturers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LecturerId",
                table: "Claims",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Claims",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "Claims",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HR",
                columns: table => new
                {
                    HRId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR", x => x.HRId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, "Lofentse13@CMCSLEC.com", "Lofentse", "Moagi", "BD5CE2FCF52FD92709E65388151D63CEFC65B75F0BBC7DCC8D401764BDFAE427", "Lecturer" },
                    { 2, "Karabo28@CMCSLEC.com", "Karabo", "Kgoebane", "B7E307660E1611CB42BCB28E4BB4A6465CCB5EC2E028CA4BE8B84E8787929A38", "Lecturer" },
                    { 3, "Claudia06@CMCSLEC.com", "Claudia", "Brander", "2FB451F9569989E892CED96048464D6739285A0A5FE00A1A12C47E9D3AF93762", "Lecturer" },
                    { 4, "Co-ordinator@CoordCMCS.com", "Steph", "Curry", "FD7D77DE225B32549F1F537D378526AD307F575C5B6D66370A50C80CF54E081A", "Coordinator" },
                    { 5, "Manager@ManCMCS.com", "Ethan", "Hunt", "8C454536B6E2B8B29A1D839AA3C5CCF0AB57A590D619739B23A32D11585220C9", "Manager" },
                    { 6, "HR@ResourcesCMCS.com", "Shrek", "Fiona", "93759AF6F455B1610E615483CF5EA847B0B7248055C16BE328C9F292D8695A9C", "HR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_HRId",
                table: "Lecturers",
                column: "HRId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_HR_HRId",
                table: "Lecturers",
                column: "HRId",
                principalTable: "HR",
                principalColumn: "HRId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_HR_HRId",
                table: "Lecturers");

            migrationBuilder.DropTable(
                name: "HR");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_HRId",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "HRId",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "Claims");

            migrationBuilder.AlterColumn<int>(
                name: "LecturerId",
                table: "Claims",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Lecturers_LecturerId",
                table: "Claims",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId");
        }
    }
}
