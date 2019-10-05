using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddOrderVarinatNameToVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ShoppingCartLineTextOptions_ShoppingCartLines_ShoppingCartLi~",
            //    table: "ShoppingCartLineTextOptions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ShoppingCartLineVariantValue_ShoppingCartLines_ShoppingCartL~",
            //    table: "ShoppingCartLineVariantValue");

            //migrationBuilder.RenameColumn(
            //    name: "LineId",
            //    table: "ShoppingCartLineVariantValue",
            //    newName: "ShoppingCartLineId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ShoppingCartLineVariantValue_LineId",
            //    table: "ShoppingCartLineVariantValue",
            //    newName: "IX_ShoppingCartLineVariantValue_ShoppingCartLineId");

            //migrationBuilder.RenameColumn(
            //    name: "LineId",
            //    table: "ShoppingCartLineTextOptions",
            //    newName: "ShoppingCartLineId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ShoppingCartLineTextOptions_LineId",
            //    table: "ShoppingCartLineTextOptions",
            //    newName: "IX_ShoppingCartLineTextOptions_ShoppingCartLineId");

            migrationBuilder.AddColumn<string>(
                name: "VarinatName",
                table: "OrderLineVariantValue",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderLineTextOptions",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartLineTextOptions_ShoppingCartLines_ShoppingCartLi~",
                table: "ShoppingCartLineTextOptions",
                column: "ShoppingCartLineId",
                principalTable: "ShoppingCartLines",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartLineVariantValue_ShoppingCartLines_ShoppingCartL~",
                table: "ShoppingCartLineVariantValue",
                column: "ShoppingCartLineId",
                principalTable: "ShoppingCartLines",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartLineTextOptions_ShoppingCartLines_ShoppingCartLi~",
                table: "ShoppingCartLineTextOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartLineVariantValue_ShoppingCartLines_ShoppingCartL~",
                table: "ShoppingCartLineVariantValue");

            migrationBuilder.DropColumn(
                name: "VarinatName",
                table: "OrderLineVariantValue");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderLineTextOptions");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartLineId",
                table: "ShoppingCartLineVariantValue",
                newName: "LineId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartLineVariantValue_ShoppingCartLineId",
                table: "ShoppingCartLineVariantValue",
                newName: "IX_ShoppingCartLineVariantValue_LineId");

            migrationBuilder.RenameColumn(
                name: "ShoppingCartLineId",
                table: "ShoppingCartLineTextOptions",
                newName: "LineId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartLineTextOptions_ShoppingCartLineId",
                table: "ShoppingCartLineTextOptions",
                newName: "IX_ShoppingCartLineTextOptions_LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartLineTextOptions_ShoppingCartLines_LineId",
                table: "ShoppingCartLineTextOptions",
                column: "LineId",
                principalTable: "ShoppingCartLines",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartLineVariantValue_ShoppingCartLines_LineId",
                table: "ShoppingCartLineVariantValue",
                column: "LineId",
                principalTable: "ShoppingCartLines",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
