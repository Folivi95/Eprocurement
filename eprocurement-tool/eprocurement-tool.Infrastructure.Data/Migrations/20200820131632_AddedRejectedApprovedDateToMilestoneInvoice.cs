using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddedRejectedApprovedDateToMilestoneInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "MilestoneInvoices",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedDate",
                table: "MilestoneInvoices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MilestoneInvoices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "MilestoneInvoices");

            migrationBuilder.DropColumn(
                name: "DeclinedDate",
                table: "MilestoneInvoices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MilestoneInvoices");
        }
    }
}
