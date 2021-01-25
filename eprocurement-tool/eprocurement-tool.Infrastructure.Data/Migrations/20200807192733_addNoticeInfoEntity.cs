using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class addNoticeInfoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoticeInformations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    SubmissionDeadline = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Organization = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: false),
                    ProcurementplanActivityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticeInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NoticeInformations_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NoticeInformations_ProcurementPlanActivities_ProcurementplanActivityId",
                        column: x => x.ProcurementplanActivityId,
                        principalTable: "ProcurementPlanActivities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoticeInformations_CreatedById",
                table: "NoticeInformations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_NoticeInformations_ProcurementplanActivityId",
                table: "NoticeInformations",
                column: "ProcurementplanActivityId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "NoticeInformations");
        }
    }
}
