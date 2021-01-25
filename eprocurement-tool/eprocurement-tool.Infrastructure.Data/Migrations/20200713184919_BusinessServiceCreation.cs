using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EGPS.Infrastructure.Data.Migrations
{
    public partial class BusinessServiceCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessCategoryID = table.Column<Guid>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedByID = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessServices_Users_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BusinessServices_BusinessCategories_BusinessCategoryID",
                        column: x => x.BusinessCategoryID,
                        principalTable: "BusinessCategories",
                        principalColumn: "BusinessCategoryID",
                        onDelete: ReferentialAction.NoAction);
                });
            //migrationBuilder.DropForeignKey(
            //    name: "FK_BusinessServices_BusinessCategories_BusinessCategoryID",
            //    table: "BusinessServices");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_BusinessServices_Users_CreatedByID",
            //    table: "BusinessServices");

            //migrationBuilder.RenameTable(
            //    name: "BusinessServices",
            //    newName: "BusinessServices");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "CreatedByID",
            //    table: "BusinessServices",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "BusinessCategoryID",
            //    table: "BusinessServices",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldNullable: true);

            //migrationBuilder.AddColumn<Guid>(
            //    name: "Id",
            //    table: "BusinessServices",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "CreateAt",
            //    table: "BusinessServices",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<bool>(
            //    name: "Deleted",
            //    table: "BusinessServices",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "DeletedAt",
            //    table: "BusinessServices",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Description",
            //    table: "BusinessServices",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Title",
            //    table: "BusinessServices",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "UpdatedAt",
            //    table: "BusinessServices",
            //    nullable: true);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_BusinessServices",
            //    table: "BusinessServices",
            //    column: "Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessServices_BusinessCategoryID",
            //    table: "BusinessServices",
            //    column: "BusinessCategoryID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessServices_CreatedByID",
            //    table: "BusinessServices",
            //    column: "CreatedByID");


            //migrationBuilder.AddForeignKey(
            //    name: "FK_BusinessServices_BusinessCategories_BusinessCategoryID",
            //    table: "BusinessServices",
            //    column: "BusinessCategoryID",
            //    principalTable: "BusinessCategories",
            //    principalColumn: "BusinessCategoryID",
            //    onDelete: ReferentialAction.NoAction);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_BusinessServices_Users_CreatedByID",
            //    table: "BusinessServices",
            //    column: "CreatedByID",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessServices_BusinessCategories_BusinessCategoryID",
                table: "BusinessServices");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessServices_Users_CreatedByID",
                table: "BusinessServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessServices",
                table: "BusinessServices");

            migrationBuilder.DropIndex(
                name: "IX_BusinessServices_BusinessCategoryID",
                table: "BusinessServices");

            migrationBuilder.DropIndex(
                name: "IX_BusinessServices_CreatedByID",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BusinessServices");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BusinessServices");

            migrationBuilder.RenameTable(
                name: "BusinessServices",
                newName: "BusinessService");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByID",
                table: "BusinessService",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "BusinessCategoryID",
                table: "BusinessService",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessService_BusinessCategories_BusinessCategoryID",
                table: "BusinessService",
                column: "BusinessCategoryID",
                principalTable: "BusinessCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessService_Users_CreatedByID",
                table: "BusinessService",
                column: "CreatedByID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

        }
    }
}
