using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "94c652e6-d468-460b-b010-a05fb25bcf9b", "AQAAAAIAAYagAAAAEA56f4ww+4zZtLyYSK/+ZyFlPHq1AnsXC+t6NwNPty1hx2QtX+pUOWdNGAYQbgcxoQ==", "4678eaad-0726-4520-b7bd-ec1787180291" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4eee981c-9278-4078-b815-dc99c7e6077c", "AQAAAAIAAYagAAAAEIQPyq1MqNUYJP4pZLd9CJr0dGdZYenANedn4kiSFj5phSFAOAuZXBOdKnIP+jrz3A==", "56aff46b-acb5-4da8-9e0c-2e65689c946c" });
        }
    }
}
