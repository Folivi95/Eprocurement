using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddProcurementPlanModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcurementPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessCategoryId = table.Column<Guid>(nullable: false),
                    ProcessTypeId = table.Column<Guid>(nullable: false),
                    MethodId = table.Column<Guid>(nullable: false),
                    EstimatedAmountInNaira = table.Column<double>(nullable: false),
                    EstimatedAmountInDollars = table.Column<double>(nullable: false),
                    QualificationMethod = table.Column<string>(nullable: true),
                    ReviewMethodId = table.Column<Guid>(nullable: false),
                    MinistryId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PackageNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementPlans", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcurementPlans");
        }
    }
}
