using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class updateNoticeInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoticeInformations_ProcurementPlanActivities_ProcurementplanActivityId",
                table: "NoticeInformations");

            migrationBuilder.DropIndex(
                name: "IX_NoticeInformations_ProcurementplanActivityId",
                table: "NoticeInformations");

            migrationBuilder.DropColumn(
                name: "ProcurementplanActivityId",
                table: "NoticeInformations");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDate",
                table: "ProcurementPlanActivities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevisedDate",
                table: "ProcurementPlanActivities",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementPlanId",
                table: "NoticeInformations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Ministries",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_NoticeInformations_ProcurementPlanId",
                table: "NoticeInformations",
                column: "ProcurementPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoticeInformations_ProcurementPlans_ProcurementPlanId",
                table: "NoticeInformations",
                column: "ProcurementPlanId",
                principalTable: "ProcurementPlans",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoticeInformations_ProcurementPlans_ProcurementPlanId",
                table: "NoticeInformations");

            migrationBuilder.DropIndex(
                name: "IX_NoticeInformations_ProcurementPlanId",
                table: "NoticeInformations");

            migrationBuilder.DropColumn(
                name: "ActualDate",
                table: "ProcurementPlanActivities");

            migrationBuilder.DropColumn(
                name: "RevisedDate",
                table: "ProcurementPlanActivities");

            migrationBuilder.DropColumn(
                name: "ProcurementPlanId",
                table: "NoticeInformations");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementplanActivityId",
                table: "NoticeInformations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "Ministries",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoticeInformations_ProcurementplanActivityId",
                table: "NoticeInformations",
                column: "ProcurementplanActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoticeInformations_ProcurementPlanActivities_ProcurementplanActivityId",
                table: "NoticeInformations",
                column: "ProcurementplanActivityId",
                principalTable: "ProcurementPlanActivities",
                principalColumn: "Id");
        }
    }
}
