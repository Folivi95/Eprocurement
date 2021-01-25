using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateProcurementPlanEntitiy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessCategoryId",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "QualificationMethod",
                table: "ProcurementPlans");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementCategoryId",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementMethodId",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "QualificationMethodId",
                table: "ProcurementPlans",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcurementCategoryId",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "ProcurementMethodId",
                table: "ProcurementPlans");

            migrationBuilder.DropColumn(
                name: "QualificationMethodId",
                table: "ProcurementPlans");

            migrationBuilder.AddColumn<Guid>(
                name: "BusinessCategoryId",
                table: "ProcurementPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MethodId",
                table: "ProcurementPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "QualificationMethod",
                table: "ProcurementPlans",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
