using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddSeoDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HtmlDescription",
                table: "CategorySubs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "CategorySubs",
                maxLength: 155,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HtmlDescription",
                table: "CategoryDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "CategoryDetails",
                maxLength: 155,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HtmlDescription",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Categories",
                maxLength: 155,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HtmlDescription",
                table: "CategorySubs");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "CategorySubs");

            migrationBuilder.DropColumn(
                name: "HtmlDescription",
                table: "CategoryDetails");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "CategoryDetails");

            migrationBuilder.DropColumn(
                name: "HtmlDescription",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Categories");
        }
    }
}
