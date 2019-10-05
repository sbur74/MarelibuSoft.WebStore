using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class VariantValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TextVariant",
                table: "ShoppingCartLines",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsShowTextVariant",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TextVariant",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextVariant",
                table: "OrderLines",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderLineVariantValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value1 = table.Column<string>(nullable: true),
                    Value2 = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    OrderLineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineVariantValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderLineVariantValue_OrderLines_OrderLineId",
                        column: x => x.OrderLineId,
                        principalTable: "OrderLines",
                        principalColumn: "OrderLineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value1 = table.Column<string>(nullable: true),
                    Value2 = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantValues_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartLineVariantValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value1 = table.Column<string>(nullable: true),
                    Value2 = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ShoppingCartLineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartLineVariantValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartLineVariantValue_ShoppingCartLines_ShoppingCartL~",
                        column: x => x.ShoppingCartLineId,
                        principalTable: "ShoppingCartLines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineVariantValue_OrderLineId",
                table: "OrderLineVariantValue",
                column: "OrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValues_ProductId",
                table: "ProductVariantValues",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartLineVariantValue_ShoppingCartLineId",
                table: "ShoppingCartLineVariantValue",
                column: "LineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLineVariantValue");

            migrationBuilder.DropTable(
                name: "ProductVariantValues");

            migrationBuilder.DropTable(
                name: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "TextVariant",
                table: "ShoppingCartLines");

            migrationBuilder.DropColumn(
                name: "IsShowTextVariant",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TextVariant",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TextVariant",
                table: "OrderLines");
        }
    }
}
