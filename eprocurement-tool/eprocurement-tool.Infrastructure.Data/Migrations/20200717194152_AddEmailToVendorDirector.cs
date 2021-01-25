using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddEmailToVendorDirector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "VendorDirectors",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_VendorDirectorCertificates_VendorDirectorId",
            //    table: "VendorDirectorCertificates",
            //    column: "VendorDirectorId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_VendorDirectorCertificates_VendorDirectors_VendorDirectorId",
            //    table: "VendorDirectorCertificates",
            //    column: "VendorDirectorId",
            //    principalTable: "VendorDirectors",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_VendorDirectorCertificates_VendorDirectors_VendorDirectorId",
            //    table: "VendorDirectorCertificates");

            //migrationBuilder.DropIndex(
            //    name: "IX_VendorDirectorCertificates_VendorDirectorId",
            //    table: "VendorDirectorCertificates");

            //migrationBuilder.DropColumn(
            //    name: "Email",
            //    table: "VendorDirectors");
        }
    }
}
