using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_courses_Course_Id",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_courses_Course_Id",
                table: "Marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courses",
                table: "courses");

            migrationBuilder.RenameTable(
                name: "courses",
                newName: "Courses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "Course_Id");

            migrationBuilder.CreateTable(
                name: "EnrollmentMarkView",
                columns: table => new
                {
                    Enrollment_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Student_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Course_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Students = table.Column<string>(type: "TEXT", nullable: false),
                    Courses = table.Column<string>(type: "TEXT", nullable: false),
                    Mark = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Remark = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Courses_Course_Id",
                table: "Enrollments",
                column: "Course_Id",
                principalTable: "Courses",
                principalColumn: "Course_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Courses_Course_Id",
                table: "Marks",
                column: "Course_Id",
                principalTable: "Courses",
                principalColumn: "Course_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Courses_Course_Id",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Courses_Course_Id",
                table: "Marks");

            migrationBuilder.DropTable(
                name: "EnrollmentMarkView");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "courses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_courses",
                table: "courses",
                column: "Course_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_courses_Course_Id",
                table: "Enrollments",
                column: "Course_Id",
                principalTable: "courses",
                principalColumn: "Course_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_courses_Course_Id",
                table: "Marks",
                column: "Course_Id",
                principalTable: "courses",
                principalColumn: "Course_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
