using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class EditNotificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notifications_Users_UserId1",
            //    table: "Notifications");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notifications_UserId1",
            //    table: "Notifications");

            //migrationBuilder.DropColumn(
            //    name: "UserId1",
            //    table: "Notifications");

            //migrationBuilder.AddColumn<Guid>(
            //    name: "UserId",
            //    table: "Notifications",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notifications_UserId",
            //    table: "Notifications",
            //    column: "UserId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notifications_Users_UserId",
            //    table: "Notifications",
            //    column: "UserId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");
        }
    }
}
