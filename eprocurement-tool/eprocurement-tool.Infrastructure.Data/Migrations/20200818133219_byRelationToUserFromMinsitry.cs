using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class byRelationToUserFromMinsitry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            //migrationBuilder.DropIndex(
            //    name: "IX_Users_MinistryId",
            //    table: "Users");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Ministries_CreatedById",
            //    table: "Ministries",
            //    column: "CreatedById",
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Ministries_Users_CreatedById",
            //    table: "Ministries",
            //    column: "CreatedById",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        
            //migrationBuilder.DropIndex(
            //    name: "IX_Ministries_CreatedById",
            //    table: "Ministries");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_MinistryId",
            //    table: "Users",
            //    column: "MinistryId",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Ministries_MinistryId",
                table: "Users",
                column: "MinistryId",
                principalTable: "Ministries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
