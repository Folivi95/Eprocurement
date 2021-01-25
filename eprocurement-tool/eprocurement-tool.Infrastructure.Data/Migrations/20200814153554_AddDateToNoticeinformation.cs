using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddDateToNoticeinformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDate",
                table: "NoticeInformations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevisedDate",
                table: "NoticeInformations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDate",
                table: "NoticeInformations");

            migrationBuilder.DropColumn(
                name: "RevisedDate",
                table: "NoticeInformations");
        }
    }
}
