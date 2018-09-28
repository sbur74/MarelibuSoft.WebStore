using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarelibuSoft.WebStore.Migrations
{
    public partial class removeDoBfromCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "WeHaveYourOrderViewModel",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShipPrice",
                table: "WeHaveYourOrderViewModel",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ShipPriceName",
                table: "WeHaveYourOrderViewModel",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShippToAddressViewModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    AdditionalAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    IsMainAddress = table.Column<bool>(nullable: false),
                    CountryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippToAddressViewModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WeHaveYourOrderLineViewModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImagePath = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    ProductNumber = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    ProductUnit = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
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
                name: "OrderViewModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    Payment = table.Column<string>(nullable: true),
                    IsPayed = table.Column<bool>(nullable: false),
                    IsSend = table.Column<bool>(nullable: false),
                    Shippingdate = table.Column<DateTime>(nullable: false),
                    ShippToAddressID = table.Column<int>(nullable: true),
                    IsClosed = table.Column<bool>(nullable: false),
                    EMail = table.Column<string>(nullable: true),
                    CustomerFirstName = table.Column<string>(nullable: true),
                    CutomerLastName = table.Column<string>(nullable: true),
                    ExceptLawConditions = table.Column<bool>(nullable: false),
                    ShippingPriceName = table.Column<string>(nullable: true),
                    ShippingPriceAtOrder = table.Column<decimal>(nullable: false),
                    ShippingPeriodString = table.Column<string>(nullable: true),
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
                name: "OrderLineViewModel",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Position = table.Column<int>(nullable: false),
                    OrderPrice = table.Column<decimal>(nullable: false),
                    OrderQuantity = table.Column<decimal>(nullable: false),
                    OrderLineTotal = table.Column<decimal>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    ProductNumber = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    OrderUnit = table.Column<string>(nullable: true),
                    OrderViewModelID = table.Column<Guid>(nullable: true)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLineViewModel");

            migrationBuilder.DropTable(
                name: "WeHaveYourOrderLineViewModel");

            migrationBuilder.DropTable(
                name: "OrderViewModel");

            migrationBuilder.DropTable(
                name: "ShippToAddressViewModel");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "WeHaveYourOrderViewModel");

            migrationBuilder.DropColumn(
                name: "ShipPrice",
                table: "WeHaveYourOrderViewModel");

            migrationBuilder.DropColumn(
                name: "ShipPriceName",
                table: "WeHaveYourOrderViewModel");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Customers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
