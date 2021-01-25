using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddedProcurementPlanNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcurementPlanNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    StateCode = table.Column<string>(nullable: true),
                    MinistryCode = table.Column<string>(nullable: true),
                    ProcurementCategoryCode = table.Column<string>(nullable: true),
                    ProcurementMethodCode = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    PlanNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementPlanNumbers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcurementPlanNumbers");
        }
    }
}
