using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangePrincipalKeyBetweenStudentsAndStudentCoursesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Students_StudetnID",
                table: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserID",
                table: "Students");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Students_UserID",
                table: "Students",
                column: "UserID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Students_StudetnID",
                table: "StudentCourses",
                column: "StudetnID",
                principalTable: "Students",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourses_Students_StudetnID",
                table: "StudentCourses");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Students_UserID",
                table: "Students");

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

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserID",
                table: "Students",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourses_Students_StudetnID",
                table: "StudentCourses",
                column: "StudetnID",
                principalTable: "Students",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
