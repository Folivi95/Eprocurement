using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class EditNotification1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<Guid>(
            //    name: "UserId1",
            //    table: "Notifications",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notifications_UserId1",
            //    table: "Notifications",
            //    column: "UserId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Notifications_Users_UserId1",
            //    table: "Notifications",
            //    column: "UserId1",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.NoAction);

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Notifications_Users_UserId1",
            //    table: "Notifications");

            //migrationBuilder.DropIndex(
            //    name: "IX_Notifications_UserId1",
            //    table: "Notifications");

            //migrationBuilder.DropColumn(
            //    name: "UserId1",
            //    table: "Notifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId1",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId1",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Notifications");
        }
    }
}
