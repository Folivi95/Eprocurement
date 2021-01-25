using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class ModifiedProjectMilestone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MilestoneInvoices_ProjectMileStoneId",
                table: "MilestoneInvoices");

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneInvoices_ProjectMileStoneId",
                table: "MilestoneInvoices",
                column: "ProjectMileStoneId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MilestoneInvoices_ProjectMileStoneId",
                table: "MilestoneInvoices");

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneInvoices_ProjectMileStoneId",
                table: "MilestoneInvoices",
                column: "ProjectMileStoneId");
        }
    }
}
