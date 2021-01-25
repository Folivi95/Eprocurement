using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateVendorRegistrationCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<DateTime>(
            //     name: "CreateAt",
            //     table: "VendorRegistrationCategories",
            //     nullable: false,
            //     defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            // migrationBuilder.AddColumn<DateTime>(
            //     name: "UpdatedAt",
            //     table: "VendorRegistrationCategories",
            //     nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "VendorRegistrationCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "VendorRegistrationCategories");
        }
    }
}
