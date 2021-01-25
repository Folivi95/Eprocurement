using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class addVendorProcurementsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorProcurements",
                columns: table => new
                {
                    ProcurementPlanId = table.Column<Guid>(nullable: false),
                    VendorId = table.Column<Guid>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    BidPrice = table.Column<decimal>(nullable: true),
                    EvaluatedPrice = table.Column<decimal>(nullable: true),
                    Type = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorProcurements", x => new { x.VendorId, x.ProcurementPlanId });
                    table.ForeignKey(
                        name: "FK_VendorProcurements_ProcurementPlans_ProcurementPlanId",
                        column: x => x.ProcurementPlanId,
                        principalTable: "ProcurementPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorProcurements_VendorProfiles_VendorId",
                        column: x => x.VendorId,
                        principalTable: "VendorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorProcurements_ProcurementPlanId",
                table: "VendorProcurements",
                column: "ProcurementPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorProcurements");
        }
    }
}
