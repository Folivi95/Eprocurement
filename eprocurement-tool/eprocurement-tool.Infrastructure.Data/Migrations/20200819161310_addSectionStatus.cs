using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class addSectionStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionOne",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SectionThree",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SectionTwo",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SectionOne",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "SectionThree",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "SectionTwo",
                table: "ProcurementPlans");
        }
    }
}
