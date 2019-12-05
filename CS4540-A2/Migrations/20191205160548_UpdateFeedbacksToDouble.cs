using Microsoft.EntityFrameworkCore.Migrations;

namespace CS4540A2.Migrations.LOS
{
    public partial class UpdateFeedbacksToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CourseOverallRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "CourseOrganizedRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "CourseObjMetRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "CourseEffectiveRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CourseOverallRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CourseOrganizedRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CourseObjMetRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CourseEffectiveRate",
                table: "Feedbacks",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
