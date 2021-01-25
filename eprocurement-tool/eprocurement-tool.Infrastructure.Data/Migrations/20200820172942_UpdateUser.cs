using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMileStones_Projects_ProjectId",
                table: "ProjectMileStones");

            migrationBuilder.AddColumn<int>(
                name: "VendorRegStage",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMileStones_Projects_ProjectId",
                table: "ProjectMileStones",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMileStones_Projects_ProjectId",
                table: "ProjectMileStones");

            migrationBuilder.DropColumn(
                name: "VendorRegStage",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMileStones_Projects_ProjectId",
                table: "ProjectMileStones",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
