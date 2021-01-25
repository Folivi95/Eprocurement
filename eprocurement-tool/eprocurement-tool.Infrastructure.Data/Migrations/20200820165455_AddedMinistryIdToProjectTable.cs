using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddedMinistryIdToProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMileStones_Users_CreatedById",
                table: "ProjectMileStones");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Contracts_ContractId",
                table: "Projects");

            migrationBuilder.AddColumn<Guid>(
                name: "MinistryId",
                table: "Projects",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_MinistryId",
                table: "Projects",
                column: "MinistryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMileStones_Users_CreatedById",
                table: "ProjectMileStones",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Contracts_ContractId",
                table: "Projects",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Projects_Ministries_MinistryId",
            //    table: "Projects",
            //    column: "MinistryId",
            //    principalTable: "Ministries",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMileStones_Users_CreatedById",
                table: "ProjectMileStones");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Contracts_ContractId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Ministries_MinistryId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_MinistryId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MinistryId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMileStones_Users_CreatedById",
                table: "ProjectMileStones",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Contracts_ContractId",
                table: "Projects",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }
    }
}
