using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class removeViewModelClassesFromContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLineViewModel");

            migrationBuilder.DropTable(
                name: "WeHaveYourOrderLineViewModel");

            migrationBuilder.DropTable(
                name: "OrderViewModel");

            migrationBuilder.DropTable(
                name: "WeHaveYourOrderViewModel");

            migrationBuilder.DropTable(
                name: "ShippToAddressViewModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippToAddressViewModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdditionalAddress = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    IsMainAddress = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippToAddressViewModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WeHaveYourOrderViewModel",
                columns: table => new
                {
                    OrderID = table.Column<Guid>(nullable: false),
                    BankID = table.Column<int>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderNo = table.Column<string>(nullable: true),
                    OrderPaymend = table.Column<string>(nullable: true),
                    OrderShippingAddressID = table.Column<int>(nullable: true),
                    OrderShippingPeriod = table.Column<string>(nullable: true),
                    OrderThankYou = table.Column<string>(nullable: true),
                    OrderTotal = table.Column<decimal>(nullable: false),
                    ShipPrice = table.Column<decimal>(nullable: false),
                    ShipPriceName = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "OrderViewModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CustomerFirstName = table.Column<string>(nullable: true),
                    CutomerLastName = table.Column<string>(nullable: true),
                    EMail = table.Column<string>(nullable: true),
                    ExceptLawConditions = table.Column<bool>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    IsPayed = table.Column<bool>(nullable: false),
                    IsSend = table.Column<bool>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    Payment = table.Column<string>(nullable: true),
                    ShippToAddressID = table.Column<int>(nullable: true),
                    ShippingPeriodString = table.Column<string>(nullable: true),
                    ShippingPriceAtOrder = table.Column<decimal>(nullable: false),
                    ShippingPriceName = table.Column<string>(nullable: true),
                    Shippingdate = table.Column<DateTime>(nullable: false),
                    Total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderViewModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderViewModel_ShippToAddressViewModel_ShippToAddressID",
                        column: x => x.ShippToAddressID,
                        principalTable: "ShippToAddressViewModel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeHaveYourOrderLineViewModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImagePath = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    ProductNumber = table.Column<int>(nullable: false),
                    ProductUnit = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    WeHaveYourOrderViewModelOrderID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeHaveYourOrderLineViewModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WeHaveYourOrderLineViewModel_WeHaveYourOrderViewModel_WeHave~",
                        column: x => x.WeHaveYourOrderViewModelOrderID,
                        principalTable: "WeHaveYourOrderViewModel",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderLineViewModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<string>(nullable: true),
                    OrderLineTotal = table.Column<decimal>(nullable: false),
                    OrderPrice = table.Column<decimal>(nullable: false),
                    OrderQuantity = table.Column<decimal>(nullable: false),
                    OrderUnit = table.Column<string>(nullable: true),
                    OrderViewModelID = table.Column<Guid>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    ProductNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLineViewModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderLineViewModel_OrderViewModel_OrderViewModelID",
                        column: x => x.OrderViewModelID,
                        principalTable: "OrderViewModel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineViewModel_OrderViewModelID",
                table: "OrderLineViewModel",
                column: "OrderViewModelID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderViewModel_ShippToAddressID",
                table: "OrderViewModel",
                column: "ShippToAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_WeHaveYourOrderLineViewModel_WeHaveYourOrderViewModelOrderID",
                table: "WeHaveYourOrderLineViewModel",
                column: "WeHaveYourOrderViewModelOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_WeHaveYourOrderViewModel_BankID",
                table: "WeHaveYourOrderViewModel",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_WeHaveYourOrderViewModel_OrderShippingAddressID",
                table: "WeHaveYourOrderViewModel",
                column: "OrderShippingAddressID");
        }
    }
}
