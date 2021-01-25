using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class addReviewTableModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Users_CreatedById",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProcurementPlanDocumentId",
                table: "Reviews",
                newName: "IX_Reviews_ProcurementPlanDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ProcurementPlanActivityId",
                table: "Reviews",
                newName: "IX_Reviews_ProcurementPlanActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_CreatedById",
                table: "Reviews",
                newName: "IX_Reviews_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_CreatedById",
                table: "Reviews",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "Reviews",
                column: "ProcurementPlanActivityId",
                principalTable: "ProcurementPlanActivities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Reviews",
                column: "ProcurementPlanDocumentId",
                principalTable: "ProcurementPlanDocuments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_CreatedById",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProcurementPlanDocumentId",
                table: "Review",
                newName: "IX_Review_ProcurementPlanDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ProcurementPlanActivityId",
                table: "Review",
                newName: "IX_Review_ProcurementPlanActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CreatedById",
                table: "Review",
                newName: "IX_Review_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Users_CreatedById",
                table: "Review",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ProcurementPlanActivities_ProcurementPlanActivityId",
                table: "Review",
                column: "ProcurementPlanActivityId",
                principalTable: "ProcurementPlanActivities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ProcurementPlanDocuments_ProcurementPlanDocumentId",
                table: "Review",
                column: "ProcurementPlanDocumentId",
                principalTable: "ProcurementPlanDocuments",
                principalColumn: "Id");
        }
    }
}
