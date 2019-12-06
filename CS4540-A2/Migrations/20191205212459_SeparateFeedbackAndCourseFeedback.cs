using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CS4540A2.Migrations.LOS
{
    public partial class SeparateFeedbackAndCourseFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseFeedbackfId",
                table: "Feedbacks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseFeedbackfId1",
                table: "Feedbacks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseFeedbackfId2",
                table: "Feedbacks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseFeedbackfId3",
                table: "Feedbacks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseFeedbacks",
                columns: table => new
                {
                    fId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cId = table.Column<int>(nullable: false),
                    AvgCourseEffectiveRate = table.Column<double>(nullable: false),
                    AvgCourseOrganizedRate = table.Column<double>(nullable: false),
                    AvgCourseObjMetRate = table.Column<double>(nullable: false),
                    AvgCourseOverallRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseFeedbacks", x => x.fId);
                    table.ForeignKey(
                        name: "FK_CourseFeedbacks_Courses_cId",
                        column: x => x.cId,
                        principalTable: "Courses",
                        principalColumn: "CId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CourseFeedbackfId",
                table: "Feedbacks",
                column: "CourseFeedbackfId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CourseFeedbackfId1",
                table: "Feedbacks",
                column: "CourseFeedbackfId1");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CourseFeedbackfId2",
                table: "Feedbacks",
                column: "CourseFeedbackfId2");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CourseFeedbackfId3",
                table: "Feedbacks",
                column: "CourseFeedbackfId3");

            migrationBuilder.CreateIndex(
                name: "IX_CourseFeedbacks_cId",
                table: "CourseFeedbacks",
                column: "cId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId",
                table: "Feedbacks",
                column: "CourseFeedbackfId",
                principalTable: "CourseFeedbacks",
                principalColumn: "fId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId1",
                table: "Feedbacks",
                column: "CourseFeedbackfId1",
                principalTable: "CourseFeedbacks",
                principalColumn: "fId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId2",
                table: "Feedbacks",
                column: "CourseFeedbackfId2",
                principalTable: "CourseFeedbacks",
                principalColumn: "fId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId3",
                table: "Feedbacks",
                column: "CourseFeedbackfId3",
                principalTable: "CourseFeedbacks",
                principalColumn: "fId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId1",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId2",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_CourseFeedbacks_CourseFeedbackfId3",
                table: "Feedbacks");

            migrationBuilder.DropTable(
                name: "CourseFeedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_CourseFeedbackfId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_CourseFeedbackfId1",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_CourseFeedbackfId2",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_CourseFeedbackfId3",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "CourseFeedbackfId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "CourseFeedbackfId1",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "CourseFeedbackfId2",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "CourseFeedbackfId3",
                table: "Feedbacks");
        }
    }
}
