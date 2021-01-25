using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdatedEstimatedValueSpending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatatedValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "EstimatatedValue",
                table: "ProjectMileStones");

            migrationBuilder.DropColumn(
                name: "EstimatatedValue",
                table: "MilestoneTasks");

            migrationBuilder.AddColumn<double>(
                name: "EstimatedValue",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EstimatedValue",
                table: "ProjectMileStones",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EstimatedValue",
                table: "MilestoneTasks",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedValue",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "EstimatedValue",
                table: "ProjectMileStones");

            migrationBuilder.DropColumn(
                name: "EstimatedValue",
                table: "MilestoneTasks");

            migrationBuilder.AddColumn<double>(
                name: "EstimatatedValue",
                table: "Projects",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EstimatatedValue",
                table: "ProjectMileStones",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EstimatatedValue",
                table: "MilestoneTasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
