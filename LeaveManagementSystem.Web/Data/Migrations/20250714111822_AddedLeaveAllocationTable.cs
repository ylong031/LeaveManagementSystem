using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLeaveAllocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveAllocations_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId, /*Sets the foreign key column in this table.*/
                        principalTable: "AspNetUsers",
                        principalColumn: "Id", /*Links to the Id column of the AspNetUsers table.*/
                        onDelete: ReferentialAction.Cascade);/*If a user is deleted, related leave allocations are also deleted.*/             
                    /* ReferentialAction.restrict here would mean that if you try to delete the employee because you have some related records,
                * you cannot delete the employee unless you delete all the related records.
                * So that would make it very difficult for you to even accidentally remove the primary record.*/
                    table.ForeignKey(
                        name: "FK_LeaveAllocations_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveAllocations_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c9b14dd3-cf3f-445b-baf9-e5a8a82c72cd", "AQAAAAIAAYagAAAAEOfHBsrVYwwWTkzROOnCJhDLXbtR/HyJEQaKN94XC9+CWCpghrMve34rvGuDc8rBsA==", "9ec2b53d-df7c-4014-bf81-e747b6a5dfd6" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAllocations_EmployeeId",
                table: "LeaveAllocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAllocations_LeaveTypeId",
                table: "LeaveAllocations",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAllocations_PeriodId",
                table: "LeaveAllocations",
                column: "PeriodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveAllocations");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "371fde73-4b3e-4038-8c02-f68bcbf32497",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc5db8c2-3c64-4df9-aa61-26dfe73916bf", "AQAAAAIAAYagAAAAEH2A9gm9u/RnqQBbER0Hx4d5EZTMUAMrcKkvcOuT+b66I0xODOwDHG3NegPWH57Hpg==", "725e39f2-bcd8-4e4c-8e3c-fb52463fe34f" });
        }
    }
}
