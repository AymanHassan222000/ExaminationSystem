using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEntitiesv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "PreRequesit",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                IsMandatory = table.Column<bool>(type: "bit", nullable: false),
                MainCourseID = table.Column<int>(type: "int", nullable: false),
                PreRequisiteID = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<int>(type: "int", nullable: false),
                UpdatedBy = table.Column<int>(type: "int", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PreRequesit", x => x.ID);
                table.ForeignKey(
                    name: "FK_PreRequesit_Courses_MainCourseID",
                    column: x => x.MainCourseID,
                    principalTable: "Courses",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PreRequesit_Courses_PreRequisiteID",
                    column: x => x.PreRequisiteID,
                    principalTable: "Courses",
                    principalColumn: "ID",
                    onDelete: ReferentialAction.NoAction);
            });


            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Instructors_InstructorID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Instructors_InstructorID",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_InstructorID",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_UserID",
                table: "Instructors");

            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "Questions",
                newName: "Head");

            migrationBuilder.RenameColumn(
                name: "InstructorID",
                table: "Questions",
                newName: "Grade");

            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Instructors_UserID",
                table: "Instructors",
                column: "UserID");


            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 17, 10, 34, 312, DateTimeKind.Utc).AddTicks(1544));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 17, 10, 34, 312, DateTimeKind.Utc).AddTicks(1548));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 17, 10, 34, 312, DateTimeKind.Utc).AddTicks(1549));

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CourseID",
                table: "Questions",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequesit_MainCourseID",
                table: "PreRequesit",
                column: "MainCourseID");

            migrationBuilder.CreateIndex(
                name: "IX_PreRequesit_PreRequisiteID",
                table: "PreRequesit",
                column: "PreRequisiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Instructors_InstructorID",
                table: "Courses",
                column: "InstructorID",
                principalTable: "Instructors",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Courses_CourseID",
                table: "Questions",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Instructors_InstructorID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Courses_CourseID",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "PreRequesit");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CourseID",
                table: "Questions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Instructors_UserID",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "Head",
                table: "Questions",
                newName: "QuestionText");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Questions",
                newName: "InstructorID");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 8, 32, 54, 803, DateTimeKind.Utc).AddTicks(5930));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 8, 32, 54, 803, DateTimeKind.Utc).AddTicks(5936));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 26, 8, 32, 54, 803, DateTimeKind.Utc).AddTicks(5937));

            migrationBuilder.CreateIndex(
                name: "IX_Questions_InstructorID",
                table: "Questions",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserID",
                table: "Instructors",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Instructors_InstructorID",
                table: "Courses",
                column: "InstructorID",
                principalTable: "Instructors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Instructors_InstructorID",
                table: "Questions",
                column: "InstructorID",
                principalTable: "Instructors",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
