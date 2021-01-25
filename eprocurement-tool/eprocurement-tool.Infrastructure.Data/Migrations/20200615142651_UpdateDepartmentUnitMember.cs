using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class UpdateDepartmentUnitMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentMembers_Users_UserId",
                table: "DepartmentMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitMembers_Users_UserId",
                table: "UnitMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentMembers_Users_UserId",
                table: "DepartmentMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitMembers_Users_UserId",
                table: "UnitMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepartmentMembers_Users_UserId",
                table: "DepartmentMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitMembers_Users_UserId",
                table: "UnitMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_DepartmentMembers_Users_UserId",
                table: "DepartmentMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitMembers_Users_UserId",
                table: "UnitMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
