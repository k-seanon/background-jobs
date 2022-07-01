using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobService.SqlServer.Migrations
{
    public partial class initindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "JobStatus",
                table: "Jobs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobStatus",
                table: "Jobs",
                column: "JobStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobStatus",
                table: "Jobs");

            migrationBuilder.AlterColumn<string>(
                name: "JobStatus",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
