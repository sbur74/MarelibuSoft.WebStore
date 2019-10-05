using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddTextOptionsAndProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantValues_Products_ProductId",
                table: "ProductVariantValues");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantOptionTemplates_VariantTemplates_VariantTemplateID",
                table: "VariantOptionTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariantValues",
                table: "ProductVariantValues");

            migrationBuilder.DropColumn(
                name: "TextVariant",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ProductVariantValues",
                newName: "ProductVariantValue");

            migrationBuilder.RenameColumn(
                name: "VariantTemplateID",
                table: "VariantOptionTemplates",
                newName: "VariantTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_VariantOptionTemplates_VariantTemplateID",
                table: "VariantOptionTemplates",
                newName: "IX_VariantOptionTemplates_VariantTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantValues_ProductId",
                table: "ProductVariantValue",
                newName: "IX_ProductVariantValue_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "VariantTemplateId",
                table: "VariantOptionTemplates",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariantValue",
                table: "ProductVariantValue",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OrderLineTextOptions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    OrderLineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineTextOptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderLineTextOptions_OrderLines_OrderLineId",
                        column: x => x.OrderLineId,
                        principalTable: "OrderLines",
                        principalColumn: "OrderLineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartLineTextOptions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    ShoppingCartLineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartLineTextOptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShoppingCartLineTextOptions_ShoppingCartLines_ShoppingCartLi~",
                        column: x => x.ShoppingCartLineId,
                        principalTable: "ShoppingCartLines",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineTextOptions_OrderLineId",
                table: "OrderLineTextOptions",
                column: "OrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartLineTextOptions_ShoppingCartLineId",
                table: "ShoppingCartLineTextOptions",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantValue_Products_ProductId",
                table: "ProductVariantValue",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantOptionTemplates_VariantTemplates_VariantTemplateId",
                table: "VariantOptionTemplates",
                column: "VariantTemplateId",
                principalTable: "VariantTemplates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantValue_Products_ProductId",
                table: "ProductVariantValue");

            migrationBuilder.DropForeignKey(
                name: "FK_VariantOptionTemplates_VariantTemplates_VariantTemplateId",
                table: "VariantOptionTemplates");

            migrationBuilder.DropTable(
                name: "OrderLineTextOptions");

            migrationBuilder.DropTable(
                name: "ShoppingCartLineTextOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariantValue",
                table: "ProductVariantValue");

            migrationBuilder.RenameTable(
                name: "ProductVariantValue",
                newName: "ProductVariantValues");

            migrationBuilder.RenameColumn(
                name: "VariantTemplateId",
                table: "VariantOptionTemplates",
                newName: "VariantTemplateID");

            migrationBuilder.RenameIndex(
                name: "IX_VariantOptionTemplates_VariantTemplateId",
                table: "VariantOptionTemplates",
                newName: "IX_VariantOptionTemplates_VariantTemplateID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariantValue_ProductId",
                table: "ProductVariantValues",
                newName: "IX_ProductVariantValues_ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "VariantTemplateID",
                table: "VariantOptionTemplates",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "TextVariant",
                table: "Products",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariantValues",
                table: "ProductVariantValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantValues_Products_ProductId",
                table: "ProductVariantValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariantOptionTemplates_VariantTemplates_VariantTemplateID",
                table: "VariantOptionTemplates",
                column: "VariantTemplateID",
                principalTable: "VariantTemplates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
