using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class EditedBidEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Bids",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementProcessId",
                table: "Bids",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bids_ProcurementProcessId",
                table: "Bids",
                column: "ProcurementProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_ProcurementProcesses_ProcurementProcessId",
                table: "Bids",
                column: "ProcurementProcessId",
                principalTable: "ProcurementProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_ProcurementProcesses_ProcurementProcessId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_ProcurementProcessId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ProcurementProcessId",
                table: "Bids");

            migrationBuilder.AlterColumn<Guid>(
                name: "Title",
                table: "Bids",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
