using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateVendorBid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Ministry",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcurementCategory",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcurementType",
                table: "VendorBids",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Ministry",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "ProcurementCategory",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "ProcurementType",
                table: "VendorBids");

        }
    }
}
