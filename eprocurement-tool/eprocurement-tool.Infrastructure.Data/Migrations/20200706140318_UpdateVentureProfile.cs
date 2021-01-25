using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateVentureProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoreCompentency",
                table: "VendorProfiles");

            migrationBuilder.AddColumn<int>(
                name: "CoreCompetency",
                table: "VendorProfiles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoreCompetency",
                table: "VendorProfiles");

            migrationBuilder.AddColumn<int>(
                name: "CoreCompentency",
                table: "VendorProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
