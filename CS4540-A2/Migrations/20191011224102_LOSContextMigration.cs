using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CS4540A2.Migrations
{
    public partial class LOSContextMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Dept = table.Column<string>(maxLength: 6, nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Semester = table.Column<string>(maxLength: 2, nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CId);
                });

            migrationBuilder.CreateTable(
                name: "CourseNotes",
                columns: table => new
                {
                    CNId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(maxLength: 350, nullable: true),
                    PostDate = table.Column<DateTime>(nullable: false),
                    ProfessorFullName = table.Column<string>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    CourseCId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseNotes", x => x.CNId);
                    table.ForeignKey(
                        name: "FK_CourseNotes_Courses_CourseCId",
                        column: x => x.CourseCId,
                        principalTable: "Courses",
                        principalColumn: "CId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOS",
                columns: table => new
                {
                    LId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    CourseCId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOS", x => x.LId);
                    table.ForeignKey(
                        name: "FK_LOS_Courses_CourseCId",
                        column: x => x.CourseCId,
                        principalTable: "Courses",
                        principalColumn: "CId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOSNotes",
                columns: table => new
                {
                    LNId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(maxLength: 350, nullable: true),
                    PostDate = table.Column<DateTime>(nullable: false),
                    IsProfessorNote = table.Column<bool>(nullable: false),
                    LearningOutcomeLId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOSNotes", x => x.LNId);
                    table.ForeignKey(
                        name: "FK_LOSNotes_LOS_LearningOutcomeLId",
                        column: x => x.LearningOutcomeLId,
                        principalTable: "LOS",
                        principalColumn: "LId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseNotes_CourseCId",
                table: "CourseNotes",
                column: "CourseCId");

            migrationBuilder.CreateIndex(
                name: "IX_LOS_CourseCId",
                table: "LOS",
                column: "CourseCId");

            migrationBuilder.CreateIndex(
                name: "IX_LOSNotes_LearningOutcomeLId",
                table: "LOSNotes",
                column: "LearningOutcomeLId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseNotes");

            migrationBuilder.DropTable(
                name: "LOSNotes");

            migrationBuilder.DropTable(
                name: "LOS");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
