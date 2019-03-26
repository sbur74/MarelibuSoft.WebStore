﻿// <auto-generated />
using System;
using MarelibuSoft.WebStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MarelibuSoft.WebStore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190326192838_AddTrackingNumber")]
    partial class AddTrackingNumber
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.BankAcccount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountOwner");

                    b.Property<string>("Iban");

                    b.Property<string>("Institute");

                    b.Property<string>("SwiftBic");

                    b.HasKey("ID");

                    b.ToTable("BankAcccounts");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HtmlDescription");

                    b.Property<string>("Name");

                    b.Property<string>("SeoDescription")
                        .HasMaxLength(155);

                    b.Property<string>("SeoKeywords");

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CategoryAssignment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryDetailID");

                    b.Property<int>("CategoryID");

                    b.Property<int>("CategorySubID");

                    b.Property<int>("ProductID");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("CategoryAssignments");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CategoryDetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategorySubID");

                    b.Property<string>("HtmlDescription");

                    b.Property<string>("Name");

                    b.Property<string>("SeoDescription")
                        .HasMaxLength(155);

                    b.Property<string>("SeoKeywords");

                    b.HasKey("ID");

                    b.HasIndex("CategorySubID");

                    b.ToTable("CategoryDetails");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CategorySub", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("HtmlDescription");

                    b.Property<string>("Name");

                    b.Property<string>("SeoDescription")
                        .HasMaxLength(155);

                    b.Property<string>("SeoKeywords");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("CategorySubs");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CmsPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastChange");

                    b.Property<int>("LayoutEnum");

                    b.Property<string>("Name");

                    b.Property<string>("PageContent");

                    b.Property<string>("PageDesciption");

                    b.Property<int>("PageEnum");

                    b.Property<string>("SeoDescription");

                    b.Property<string>("SeoKeywords");

                    b.Property<int>("StatusEnum");

                    b.Property<string>("Titel");

                    b.HasKey("Id");

                    b.ToTable("CmsPages");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CmsPageDraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DraftOfPageId");

                    b.Property<DateTime>("LastChange");

                    b.Property<int>("LayoutEnum");

                    b.Property<string>("Name");

                    b.Property<string>("PageContent");

                    b.Property<string>("PageDesciption");

                    b.Property<int>("PageEnum");

                    b.Property<string>("SeoDescription");

                    b.Property<string>("SeoKeywords");

                    b.Property<int>("StatusEnum");

                    b.Property<string>("Titel");

                    b.HasKey("Id");

                    b.ToTable("CmsPageDrafts");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CmsStartPage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HeadContent");

                    b.Property<string>("LeftContent");

                    b.Property<string>("RightContent");

                    b.Property<string>("SeoDescription");

                    b.Property<string>("SeoKeywords");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("CmsStartPages");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Country", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<bool>("IsAllowedShipping");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Customer", b =>
                {
                    b.Property<Guid>("CustomerID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalAddress");

                    b.Property<string>("Address");

                    b.Property<bool>("AllowedPayByBill");

                    b.Property<string>("City");

                    b.Property<string>("CompanyName");

                    b.Property<int>("CountryId");

                    b.Property<string>("CustomerNo");

                    b.Property<string>("FirstName");

                    b.Property<string>("Name");

                    b.Property<string>("PostCode");

                    b.Property<string>("UserId");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CustomerGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<decimal>("Rabatt");

                    b.HasKey("ID");

                    b.ToTable("CustomerGroups");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CustomerGroupAssignment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CustomerID");

                    b.Property<int>("GroupID");

                    b.HasKey("ID");

                    b.HasIndex("GroupID");

                    b.ToTable("CustomerGroupAssignments");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Ebook", b =>
                {
                    b.Property<int>("EbookID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FkProductID");

                    b.Property<string>("Name");

                    b.Property<string>("Path");

                    b.Property<string>("PathFileID");

                    b.Property<string>("Version");

                    b.HasKey("EbookID");

                    b.ToTable("Ebooks");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Guest", b =>
                {
                    b.Property<int>("GuestID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalAddress");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Name");

                    b.Property<string>("PostCode");

                    b.HasKey("GuestID");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Impressum", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalAddress");

                    b.Property<string>("Address");

                    b.Property<string>("Bank");

                    b.Property<string>("Bic");

                    b.Property<string>("City");

                    b.Property<int>("CountryID");

                    b.Property<string>("EMail");

                    b.Property<string>("Iban");

                    b.Property<string>("PostCode");

                    b.Property<string>("ShopAdmin");

                    b.Property<string>("ShopName");

                    b.HasKey("ID");

                    b.ToTable("Impressums");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.LawContent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HtmlContent");

                    b.Property<int>("SiteType");

                    b.Property<string>("Titel");

                    b.HasKey("ID");

                    b.ToTable("LawContents");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Order", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CartID");

                    b.Property<Guid>("CustomerID");

                    b.Property<bool>("ExceptLawConditions");

                    b.Property<string>("FreeText");

                    b.Property<Guid>("GustID");

                    b.Property<bool>("IsClosed");

                    b.Property<bool>("IsPayed");

                    b.Property<bool>("IsSend");

                    b.Property<string>("Number");

                    b.Property<DateTime>("OrderDate");

                    b.Property<int>("PaymentID");

                    b.Property<int>("ShippingAddressId");

                    b.Property<int>("ShippingPeriodId");

                    b.Property<decimal>("ShippingPrice");

                    b.Property<int>("ShippingPriceId");

                    b.Property<DateTime>("Shippingdate");

                    b.Property<decimal>("Total");

                    b.Property<string>("TrackingNumber");

                    b.HasKey("ID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.OrderCompletionText", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("Name");

                    b.Property<int>("PaymendType");

                    b.HasKey("ID");

                    b.ToTable("OrderCompletionTexts");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.OrderLine", b =>
                {
                    b.Property<int>("OrderLineID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("OrderID");

                    b.Property<int>("Position");

                    b.Property<int>("ProductID");

                    b.Property<decimal>("Quantity");

                    b.Property<decimal>("SellBasePrice");

                    b.Property<int>("UnitID");

                    b.HasKey("OrderLineID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Paymend", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsActive");

                    b.Property<string>("LogoUrl");

                    b.Property<string>("Name");

                    b.Property<int>("PaymendType");

                    b.HasKey("ID");

                    b.ToTable("Paymends");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AvailableQuantity");

                    b.Property<int>("BasesUnitID");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<decimal>("MinimumPurchaseQuantity");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductNumber");

                    b.Property<decimal>("SecondBasePrice");

                    b.Property<int>("SecondBaseUnit");

                    b.Property<string>("SeoDescription");

                    b.Property<string>("SeoKeywords");

                    b.Property<int>("ShippingPeriod");

                    b.Property<int>("ShippingPriceType");

                    b.Property<string>("ShortDescription");

                    b.Property<int>("Size");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ProductImage", b =>
                {
                    b.Property<int>("ProductImageID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageUrl");

                    b.Property<bool>("IsMainImage");

                    b.Property<string>("Name");

                    b.Property<int>("ProductID");

                    b.HasKey("ProductImageID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ProductVariant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("ProductVariants");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ProductVariantOption", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Option");

                    b.Property<int>("ProductVariantID");

                    b.HasKey("ID");

                    b.HasIndex("ProductVariantID");

                    b.ToTable("ProductVariantOptions");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.SellAction", b =>
                {
                    b.Property<int>("SellActionID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionName");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsActive");

                    b.Property<decimal>("Percent");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("SellActionID");

                    b.ToTable("SellActions");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.SellActionItem", b =>
                {
                    b.Property<int>("SellActionItemID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FkProductID");

                    b.Property<int>("SellActionID");

                    b.HasKey("SellActionItemID");

                    b.HasIndex("SellActionID");

                    b.ToTable("SellActionItems");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShippingAddress", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalAddress");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("CompanyName");

                    b.Property<int>("CountryID");

                    b.Property<Guid>("CustomerID");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsInvoiceAddress");

                    b.Property<bool>("IsMainAddress");

                    b.Property<string>("LastName");

                    b.Property<string>("PostCode");

                    b.HasKey("ID");

                    b.ToTable("ShippingAddresses");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShippingPeriod", b =>
                {
                    b.Property<int>("ShippingPeriodID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Decription");

                    b.HasKey("ShippingPeriodID");

                    b.ToTable("ShpippingPeriods");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShippingPrice", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CountryId");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("ShippingPriceTypeId");

                    b.HasKey("ID");

                    b.HasIndex("ShippingPriceTypeId");

                    b.ToTable("ShippingPrices");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShippingPriceType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("ShippingPriceTypes");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShopContent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ShowIn");

                    b.Property<string>("Site");

                    b.Property<string>("Text");

                    b.HasKey("ID");

                    b.ToTable("ShopContents");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShopFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Filename");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<int>("ShopFileType");

                    b.HasKey("ID");

                    b.ToTable("ShopFiles");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShoppingCart", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateAt");

                    b.Property<Guid>("CustomerId");

                    b.Property<DateTime>("LastChange");

                    b.Property<string>("Number");

                    b.Property<Guid>("OrderId");

                    b.Property<string>("SessionId");

                    b.Property<int>("TabCounter");

                    b.HasKey("ID");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShoppingCartLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Position");

                    b.Property<int>("ProductID");

                    b.Property<decimal>("Quantity");

                    b.Property<decimal>("SellBasePrice");

                    b.Property<Guid>("ShoppingCartID");

                    b.Property<int>("UnitID");

                    b.HasKey("ID");

                    b.HasIndex("ShoppingCartID");

                    b.ToTable("ShoppingCartLines");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Size", b =>
                {
                    b.Property<int>("SizeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("SizeID");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.Unit", b =>
                {
                    b.Property<int>("UnitID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LongName");

                    b.Property<string>("Name");

                    b.HasKey("UnitID");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.VariantProductAssignment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductID");

                    b.Property<int>("VariantID");

                    b.HasKey("ID");

                    b.ToTable("VariantProductAssignments");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CategoryAssignment", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.Product", "Product")
                        .WithMany("CategoryAssignments")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CategoryDetail", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.CategorySub", "Sub")
                        .WithMany("Details")
                        .HasForeignKey("CategorySubID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CategorySub", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.Category", "Category")
                        .WithMany("Subs")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.CustomerGroupAssignment", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.CustomerGroup", "Group")
                        .WithMany("Assignments")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.OrderLine", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.Order", "Order")
                        .WithMany("OderLines")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ProductImage", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.Product", "Product")
                        .WithMany("ImageList")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ProductVariantOption", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.ProductVariant", "Variant")
                        .WithMany("Options")
                        .HasForeignKey("ProductVariantID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.SellActionItem", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.SellAction", "SellAction")
                        .WithMany("SellActionItems")
                        .HasForeignKey("SellActionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShippingPrice", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.ShippingPriceType", "ShippingPriceType")
                        .WithMany("Prices")
                        .HasForeignKey("ShippingPriceTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MarelibuSoft.WebStore.Models.ShoppingCartLine", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("Lines")
                        .HasForeignKey("ShoppingCartID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MarelibuSoft.WebStore.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MarelibuSoft.WebStore.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
