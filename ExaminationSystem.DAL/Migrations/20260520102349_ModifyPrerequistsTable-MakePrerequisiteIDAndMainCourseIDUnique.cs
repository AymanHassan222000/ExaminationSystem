using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPrerequistsTableMakePrerequisiteIDAndMainCourseIDUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PreRequesit_PreRequisiteID",
                table: "PreRequesit");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 10, 23, 46, 867, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 10, 23, 46, 867, DateTimeKind.Utc).AddTicks(5809));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 10, 23, 46, 867, DateTimeKind.Utc).AddTicks(5810));

            migrationBuilder.CreateIndex(
                name: "IX_PreRequesit_PreRequisiteID_MainCourseID",
                table: "PreRequesit",
                columns: new[] { "PreRequisiteID", "MainCourseID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PreRequesit_PreRequisiteID_MainCourseID",
                table: "PreRequesit");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 9, 5, 27, 672, DateTimeKind.Utc).AddTicks(5148));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 9, 5, 27, 672, DateTimeKind.Utc).AddTicks(5151));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 20, 9, 5, 27, 672, DateTimeKind.Utc).AddTicks(5152));

            migrationBuilder.CreateIndex(
                name: "IX_PreRequesit_PreRequisiteID",
                table: "PreRequesit",
                column: "PreRequisiteID");
        }
    }
}
