using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddBankAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadedAGB",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsReadedDSGVO",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "IsReadedWDBL",
                table: "Orders",
                newName: "ExceptLawConditions");

            migrationBuilder.CreateTable(
                name: "BankAcccounts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountOwner = table.Column<string>(nullable: true),
                    Institute = table.Column<string>(nullable: true),
                    Iban = table.Column<string>(nullable: true),
                    SwiftBic = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAcccounts", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAcccounts");

            migrationBuilder.RenameColumn(
                name: "ExceptLawConditions",
                table: "Orders",
                newName: "IsReadedWDBL");

            migrationBuilder.AddColumn<bool>(
                name: "IsReadedAGB",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadedDSGVO",
                table: "Orders",
                nullable: false,
                defaultValue: false);
        }
    }
}
