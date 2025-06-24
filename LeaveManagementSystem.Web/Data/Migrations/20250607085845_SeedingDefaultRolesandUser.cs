using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6114021a-4c3c-4052-bebd-ee35e7002e67", null, "Administrator", "ADMINISTRATOR" },
                    { "ad0a5c18-926f-46cf-97ef-dd3fa31366a0", null, "Supervisor", "SUPERVISOR" },
                    { "b15308f5-eba8-43b4-80fe-b885d542014e", null, "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "371fde73-4b3e-4038-8c02-f68bcbf32497", 0, "3bf204db-b6bb-402b-a5de-51f42b6fb2a4", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEJnR7EUPCOvU472nZQZHiRnX26EK3SPOHPTr3+QNTaY3uee4hQo4JeLNB6dz+W5GqA==", null, false, "6580bd71-c3a5-4554-8144-e3318fc6ae11", false, "admin@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "6114021a-4c3c-4052-bebd-ee35e7002e67", "371fde73-4b3e-4038-8c02-f68bcbf32497" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad0a5c18-926f-46cf-97ef-dd3fa31366a0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b15308f5-eba8-43b4-80fe-b885d542014e");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6114021a-4c3c-4052-bebd-ee35e7002e67", "371fde73-4b3e-4038-8c02-f68bcbf32497" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6114021a-4c3c-4052-bebd-ee35e7002e67");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497");
        }
    }
}
