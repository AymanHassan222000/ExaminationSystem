using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class ModifyExamAttemptsTableMakePrincipalKeyInStudentsTableIsUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_Students_StudentID",
                table: "ExamAttempts");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 24, 23, 3, 47, 40, DateTimeKind.Utc).AddTicks(9068));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 24, 23, 3, 47, 40, DateTimeKind.Utc).AddTicks(9085));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 24, 23, 3, 47, 40, DateTimeKind.Utc).AddTicks(9086));

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_Students_StudentID",
                table: "ExamAttempts",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamAttempts_Students_StudentID",
                table: "ExamAttempts");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ExamAttempts_Students_StudentID",
                table: "ExamAttempts",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
