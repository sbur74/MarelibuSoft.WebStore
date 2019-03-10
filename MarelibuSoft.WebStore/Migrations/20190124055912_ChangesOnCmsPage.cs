using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class ChangesOnCmsPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PageHeadline",
                table: "CmsPages",
                newName: "PageDesciption");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChange",
                table: "CmsPages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChange",
                table: "CmsPages");

            migrationBuilder.RenameColumn(
                name: "PageDesciption",
                table: "CmsPages",
                newName: "PageHeadline");
        }
    }
}
