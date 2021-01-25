using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class AddContractToProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContractId",
                table: "Projects",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ContractId",
                table: "Projects",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Contracts_ContractId",
                table: "Projects",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Contracts_ContractId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ContractId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Projects");
        }
    }
}
