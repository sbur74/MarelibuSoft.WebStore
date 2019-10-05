using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddCombiProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ShoppingCartLineVariantValue",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ShoppingCartLineVariantValue",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "Combi",
                table: "ShoppingCartLineVariantValue",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductVariant",
                table: "ShoppingCartLineVariantValue",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Combi",
                table: "ProductVariantOptions",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "OrderLineVariantValue",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderLineVariantValue",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "Combi",
                table: "OrderLineVariantValue",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductVariant",
                table: "OrderLineVariantValue",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Combi",
                table: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "ProductVariant",
                table: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "Combi",
                table: "ProductVariantOptions");

            migrationBuilder.DropColumn(
                name: "Combi",
                table: "OrderLineVariantValue");

            migrationBuilder.DropColumn(
                name: "ProductVariant",
                table: "OrderLineVariantValue");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "ShoppingCartLineVariantValue",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ShoppingCartLineVariantValue",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "OrderLineVariantValue",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "OrderLineVariantValue",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
