using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class RemoveTextOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextVariant",
                table: "ShoppingCartLines");

            migrationBuilder.DropColumn(
                name: "TextVariant",
                table: "OrderLines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TextVariant",
                table: "ShoppingCartLines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextVariant",
                table: "OrderLines",
                nullable: true);
        }
    }
}
