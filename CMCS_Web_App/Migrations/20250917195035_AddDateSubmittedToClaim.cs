using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMCS_Web_App.Migrations
{
    /// <inheritdoc />
    public partial class AddDateSubmittedToClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RatePerHour",
                table: "Lecturers",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Claims",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSubmitted",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupportingDocumentPath",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatePerHour",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "DateSubmitted",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "SupportingDocumentPath",
                table: "Claims");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Claims",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
