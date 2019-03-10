using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddCmsDraft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "CmsPages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "CmsPages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CmsPageDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DraftOfPageId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Titel = table.Column<string>(nullable: true),
                    PageDesciption = table.Column<string>(nullable: true),
                    PageContent = table.Column<string>(nullable: true),
                    PageEnum = table.Column<int>(nullable: false),
                    LayoutEnum = table.Column<int>(nullable: false),
                    StatusEnum = table.Column<int>(nullable: false),
                    LastChange = table.Column<DateTime>(nullable: false),
                    SeoDescription = table.Column<string>(nullable: true),
                    SeoKeywords = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsPageDrafts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CmsPageDrafts");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "CmsPages");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "CmsPages");
        }
    }
}
