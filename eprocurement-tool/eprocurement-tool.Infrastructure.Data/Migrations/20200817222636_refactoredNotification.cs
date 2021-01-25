using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class refactoredNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActionId",
                table: "Notifications",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ActionText",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationClass",
                table: "Notifications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ActionText",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationClass",
                table: "Notifications");
        }
    }
}
