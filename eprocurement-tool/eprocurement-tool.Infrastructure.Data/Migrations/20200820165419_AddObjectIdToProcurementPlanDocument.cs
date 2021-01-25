using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddObjectIdToProcurementPlanDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlanDocuments_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "ProcurementPlanDocuments");

            migrationBuilder.DropColumn(
                name: "ProcurementDocumentStatus",
                table: "ProcurementPlanDocuments");


            migrationBuilder.AlterColumn<Guid>(
                name: "ProcurementPlanActivityId",
                table: "ProcurementPlanDocuments",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "DocumentStatus",
                table: "ProcurementPlanDocuments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ObjectId",
                table: "ProcurementPlanDocuments",
                nullable: true);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_ProcurementPlanDocuments_ProcurementPlanActivities_ProcurementPlanActivityId",
            //     table: "ProcurementPlanDocuments",
            //     column: "ProcurementPlanActivityId",
            //     principalTable: "ProcurementPlanActivities",
            //     principalColumn: "Id",
            //     onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcurementPlanDocuments_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "ProcurementPlanDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentStatus",
                table: "ProcurementPlanDocuments");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "ProcurementPlanDocuments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProcurementPlanActivityId",
                table: "ProcurementPlanDocuments",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcurementDocumentStatus",
                table: "ProcurementPlanDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcurementPlanDocuments_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "ProcurementPlanDocuments",
                column: "ProcurementPlanActivityId",
                principalTable: "ProcurementPlanActivities",
                principalColumn: "Id");
        }
    }
}
