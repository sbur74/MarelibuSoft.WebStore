using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class ProductPublishedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedOn",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedOn",
                table: "Products");
        }
    }
}
