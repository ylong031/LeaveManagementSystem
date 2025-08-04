using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Migrations
{
    /// <inheritdoc />
    public partial class updateleaverequeststatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4eee981c-9278-4078-b815-dc99c7e6077c", "AQAAAAIAAYagAAAAEIQPyq1MqNUYJP4pZLd9CJr0dGdZYenANedn4kiSFj5phSFAOAuZXBOdKnIP+jrz3A==", "56aff46b-acb5-4da8-9e0c-2e65689c946c" });

            migrationBuilder.UpdateData(
                table: "LeaveRequestStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Cancelled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b6648e93-62fe-412a-bf32-0c414340c5dd", "AQAAAAIAAYagAAAAEMByRVQgZcH/4bg0KHM3hIsNCViKT0SLRmXJg5SlIc/x5SAUiFMSMpJ/8puZOg/Qwg==", "f6f8c756-27e2-4462-ab56-e800e2908d36" });

            migrationBuilder.UpdateData(
                table: "LeaveRequestStatuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Canceled");
        }
    }
}
