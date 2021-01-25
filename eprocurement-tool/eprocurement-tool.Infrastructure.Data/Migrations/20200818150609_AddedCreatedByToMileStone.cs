using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddedCreatedByToMileStone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneTasks_CreatedById",
                table: "MilestoneTasks",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_MilestoneTasks_Users_CreatedById",
                table: "MilestoneTasks",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MilestoneTasks_Users_CreatedById",
                table: "MilestoneTasks");

            migrationBuilder.DropIndex(
                name: "IX_MilestoneTasks_CreatedById",
                table: "MilestoneTasks");
        }
    }
}
