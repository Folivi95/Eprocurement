using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class modifyContractEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_RegistrationPlans_RegistrationPlanId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorProcurements_ProcurementPlans_ProcurementPlanId",
                table: "VendorProcurements");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorProcurements_VendorProfiles_VendorId",
                table: "VendorProcurements");

            migrationBuilder.DropColumn(
                name: "Contractor",
                table: "Contracts");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegistrationPlanId",
                table: "Contracts",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedValue",
                table: "Contracts",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<Guid>(
                name: "ContractorId",
                table: "Contracts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EvaluationCurrency",
                table: "Contracts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProcurementPlanId",
                table: "Contracts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractorId",
                table: "Contracts",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ProcurementPlanId",
                table: "Contracts",
                column: "ProcurementPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Users_ContractorId",
                table: "Contracts",
                column: "ContractorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ProcurementPlans_ProcurementPlanId",
                table: "Contracts",
                column: "ProcurementPlanId",
                principalTable: "ProcurementPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_RegistrationPlans_RegistrationPlanId",
                table: "Contracts",
                column: "RegistrationPlanId",
                principalTable: "RegistrationPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorProcurements_ProcurementPlans_ProcurementPlanId",
                table: "VendorProcurements",
                column: "ProcurementPlanId",
                principalTable: "ProcurementPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorProcurements_Users_VendorId",
                table: "VendorProcurements",
                column: "VendorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Users_ContractorId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ProcurementPlans_ProcurementPlanId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_RegistrationPlans_RegistrationPlanId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorProcurements_ProcurementPlans_ProcurementPlanId",
                table: "VendorProcurements");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorProcurements_Users_VendorId",
                table: "VendorProcurements");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ContractorId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ProcurementPlanId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "EvaluationCurrency",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ProcurementPlanId",
                table: "Contracts");

            migrationBuilder.AlterColumn<Guid>(
                name: "RegistrationPlanId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EstimatedValue",
                table: "Contracts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contractor",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_RegistrationPlans_RegistrationPlanId",
                table: "Contracts",
                column: "RegistrationPlanId",
                principalTable: "RegistrationPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorProcurements_ProcurementPlans_ProcurementPlanId",
                table: "VendorProcurements",
                column: "ProcurementPlanId",
                principalTable: "ProcurementPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorProcurements_VendorProfiles_VendorId",
                table: "VendorProcurements",
                column: "VendorId",
                principalTable: "VendorProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
