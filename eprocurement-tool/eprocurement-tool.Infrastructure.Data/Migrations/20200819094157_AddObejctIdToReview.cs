using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddObejctIdToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "Reviews");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProcurementPlanActivityId",
                table: "Reviews",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ObjectId",
                table: "Reviews",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "Reviews");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProcurementPlanActivityId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "Reviews",
                column: "ProcurementPlanActivityId",
                principalTable: "ProcurementPlanActivities",
                principalColumn: "Id");
        }
    }
}
