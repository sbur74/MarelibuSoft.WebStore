using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class UpdateVartian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ProductVariants",
                newName: "OptionName");

            migrationBuilder.AddColumn<string>(
                name: "OptionName",
                table: "VariantTemplates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionName",
                table: "VariantTemplates");

            migrationBuilder.RenameColumn(
                name: "OptionName",
                table: "ProductVariants",
                newName: "Description");
        }
    }
}
