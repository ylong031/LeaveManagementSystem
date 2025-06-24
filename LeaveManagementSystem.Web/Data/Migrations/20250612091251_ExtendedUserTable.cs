using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc5db8c2-3c64-4df9-aa61-26dfe73916bf", new DateOnly(1990, 1, 1), "Default", "Admin", "AQAAAAIAAYagAAAAEH2A9gm9u/RnqQBbER0Hx4d5EZTMUAMrcKkvcOuT+b66I0xODOwDHG3NegPWH57Hpg==", "725e39f2-bcd8-4e4c-8e3c-fb52463fe34f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3bf204db-b6bb-402b-a5de-51f42b6fb2a4", "AQAAAAIAAYagAAAAEJnR7EUPCOvU472nZQZHiRnX26EK3SPOHPTr3+QNTaY3uee4hQo4JeLNB6dz+W5GqA==", "6580bd71-c3a5-4554-8144-e3318fc6ae11" });
        }
    }
}
