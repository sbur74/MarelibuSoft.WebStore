using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddVariantImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ShoppingCartLineVariantValue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProductVariantValues",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "OrderLineVariantValue",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductVariantValues");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "OrderLineVariantValue");
        }
    }
}
