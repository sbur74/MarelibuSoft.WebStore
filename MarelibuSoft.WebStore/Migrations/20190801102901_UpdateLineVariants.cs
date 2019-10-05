using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class UpdateLineVariants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "OrderLineVariantValue");

            migrationBuilder.DropColumn(
                name: "Value1",
                table: "OrderLineVariantValue");

            migrationBuilder.RenameColumn(
                name: "Value2",
                table: "ShoppingCartLineVariantValue",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "Value2",
                table: "OrderLineVariantValue",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantOption",
                table: "ShoppingCartLineVariantValue",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantOption",
                table: "OrderLineVariantValue",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductVariantOption",
                table: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "ProductVariantOption",
                table: "OrderLineVariantValue");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "ShoppingCartLineVariantValue",
                newName: "Value2");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "OrderLineVariantValue",
                newName: "Value2");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ShoppingCartLineVariantValue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "ShoppingCartLineVariantValue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "OrderLineVariantValue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value1",
                table: "OrderLineVariantValue",
                nullable: true);
        }
    }
}
