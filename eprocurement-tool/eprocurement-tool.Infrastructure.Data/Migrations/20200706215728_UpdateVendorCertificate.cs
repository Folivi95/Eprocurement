using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateVendorCertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "IdentificationType",
                table: "VendorDirectors",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "VendorDirectors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "VendorDirectors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "VendorDirectors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "VendorDirectors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "VendorDirectorCertificates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "VendorDirectorCertificates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "VendorDirectorCertificates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "VendorDirectors");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "VendorDirectorCertificates");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "VendorDirectorCertificates");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "VendorDirectorCertificates");

            migrationBuilder.AlterColumn<string>(
                name: "IdentificationType",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentificationFile",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportPhoto",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "VendorDirectors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
