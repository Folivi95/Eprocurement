using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class EditedVendorBidEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorBids_VendorProfiles_VendorProfileId",
                table: "VendorBids");

            migrationBuilder.DropIndex(
                name: "IX_VendorBids_VendorProfileId",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "VendorProfileId",
                table: "VendorBids");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BidPrice",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EvaluatedPrice",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "VendorBids",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "VendorBids",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VendorBids_VendorId",
                table: "VendorBids",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorBids_Users_VendorId",
                table: "VendorBids",
                column: "VendorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorBids_Users_VendorId",
                table: "VendorBids");

            migrationBuilder.DropIndex(
                name: "IX_VendorBids_VendorId",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "BidPrice",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "EvaluatedPrice",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "VendorBids");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "VendorBids");

            migrationBuilder.AddColumn<Guid>(
                name: "VendorProfileId",
                table: "VendorBids",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VendorBids_VendorProfileId",
                table: "VendorBids",
                column: "VendorProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorBids_VendorProfiles_VendorProfileId",
                table: "VendorBids",
                column: "VendorProfileId",
                principalTable: "VendorProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
