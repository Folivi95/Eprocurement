using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateVendorDirectors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationFile",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportPhoto",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "VendorDirectors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "VendorDirectors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "City",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "IdentificationFile",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "PassportPhoto",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "State",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "VendorDirectors");
        }
    }
}
