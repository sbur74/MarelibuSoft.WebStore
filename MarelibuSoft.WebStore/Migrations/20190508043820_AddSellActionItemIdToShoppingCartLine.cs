using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddSellActionItemIdToShoppingCartLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellActionItemId",
                table: "ShoppingCartLines",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellActionItemId",
                table: "ShoppingCartLines");
        }
    }
}
