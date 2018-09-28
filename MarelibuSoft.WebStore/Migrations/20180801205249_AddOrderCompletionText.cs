using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class AddOrderCompletionText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderCompletionTexts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PaymendType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCompletionTexts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WeHaveYourOrderViewModel",
                columns: table => new
                {
                    OrderID = table.Column<Guid>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderNo = table.Column<string>(nullable: true),
                    OrderTotal = table.Column<decimal>(nullable: false),
                    OrderPaymend = table.Column<string>(nullable: true),
                    OrderShippingPeriod = table.Column<string>(nullable: true),
                    OrderThankYou = table.Column<string>(nullable: true),
                    BankID = table.Column<int>(nullable: true),
                    OrderShippingAddressID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeHaveYourOrderViewModel", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_WeHaveYourOrderViewModel_BankAcccounts_BankID",
                        column: x => x.BankID,
                        principalTable: "BankAcccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeHaveYourOrderViewModel_ShippingAddresses_OrderShippingAddr~",
                        column: x => x.OrderShippingAddressID,
                        principalTable: "ShippingAddresses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeHaveYourOrderViewModel_BankID",
                table: "WeHaveYourOrderViewModel",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_WeHaveYourOrderViewModel_OrderShippingAddressID",
                table: "WeHaveYourOrderViewModel",
                column: "OrderShippingAddressID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderCompletionTexts");

            migrationBuilder.DropTable(
                name: "WeHaveYourOrderViewModel");
        }
    }
}
