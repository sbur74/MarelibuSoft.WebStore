using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddCmsPages_OrderShippingTrackCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippingTrackCode",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CmsPages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Titel = table.Column<string>(nullable: true),
                    PageHeadline = table.Column<string>(nullable: true),
                    PageContent = table.Column<string>(nullable: true),
                    PageEnum = table.Column<int>(nullable: false),
                    LayoutEnum = table.Column<int>(nullable: false),
                    StatusEnum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsPages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CmsPages");

            migrationBuilder.DropColumn(
                name: "ShippingTrackCode",
                table: "Orders");
        }
    }
}
