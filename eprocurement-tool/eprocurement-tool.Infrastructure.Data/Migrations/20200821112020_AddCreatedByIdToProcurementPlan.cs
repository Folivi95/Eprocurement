using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddCreatedByIdToProcurementPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {  
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "ProcurementPlans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementPlans_CreatedById",
                table: "ProcurementPlans",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlans_Users_CreatedById",
                table: "ProcurementPlans",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlans_Users_CreatedById",
                table: "ProcurementPlans");

            migrationBuilder.DropIndex(
                name: "IX_ProcurementPlans_CreatedById",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProcurementPlans");
        }
    }
}
