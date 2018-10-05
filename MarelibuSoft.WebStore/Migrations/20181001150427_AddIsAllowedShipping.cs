using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddIsAllowedShipping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAllowedShipping",
                table: "Countries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAllowedShipping",
                table: "Countries");
        }
    }
}
