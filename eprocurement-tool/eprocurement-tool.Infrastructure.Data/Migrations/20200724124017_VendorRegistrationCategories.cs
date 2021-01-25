using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class VendorRegistrationCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorRegistrationCategories",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RegistrationPlanId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorRegistrationCategories", x => new { x.UserId, x.RegistrationPlanId });
                    table.ForeignKey(
                        name: "FK_VendorRegistrationCategories_RegistrationPlans_RegistrationPlanId",
                        column: x => x.RegistrationPlanId,
                        principalTable: "RegistrationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VendorRegistrationCategories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorRegistrationCategories_RegistrationPlanId",
                table: "VendorRegistrationCategories",
                column: "RegistrationPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorRegistrationCategories");
        }
    }
}
