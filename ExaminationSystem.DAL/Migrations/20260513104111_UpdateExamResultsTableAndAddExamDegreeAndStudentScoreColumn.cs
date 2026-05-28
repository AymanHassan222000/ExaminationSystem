using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamResultsTableAndAddExamDegreeAndStudentScoreColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalQuestions",
                table: "ExamResults",
                newName: "StudentScore");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswers",
                table: "ExamResults",
                newName: "ExamDegree");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 13, 10, 41, 8, 195, DateTimeKind.Utc).AddTicks(2226));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 13, 10, 41, 8, 195, DateTimeKind.Utc).AddTicks(2229));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 13, 10, 41, 8, 195, DateTimeKind.Utc).AddTicks(2230));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentScore",
                table: "ExamResults",
                newName: "TotalQuestions");

            migrationBuilder.RenameColumn(
                name: "ExamDegree",
                table: "ExamResults",
                newName: "CorrectAnswers");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 12, 11, 57, 31, 785, DateTimeKind.Utc).AddTicks(6566));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 12, 11, 57, 31, 785, DateTimeKind.Utc).AddTicks(6570));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 12, 11, 57, 31, 785, DateTimeKind.Utc).AddTicks(6572));
        }
    }
}
