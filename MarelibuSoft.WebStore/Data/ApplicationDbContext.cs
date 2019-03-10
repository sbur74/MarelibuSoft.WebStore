using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Models.ViewModels;
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
//using Pomelo.EntityFrameworkCore.MySql.

namespace MarelibuSoft.WebStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
		public DbSet<BankAcccount> BankAcccounts { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CategorySub> CategorySubs { get; set; }
		public DbSet<CategoryDetail> CategoryDetails { get; set; }
		public DbSet<CategoryAssignment> CategoryAssignments { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<CustomerGroup> CustomerGroups { get; set; }
		public DbSet<CustomerGroupAssignment> CustomerGroupAssignments { get; set; }
		public DbSet<CmsPage> CmsPages { get; set; }
		public DbSet<CmsPageDraft> CmsPageDrafts { get; set; }
		public DbSet<CmsStartPage> CmsStartPages { get; set; }
		public DbSet<Ebook> Ebooks { get; set; }
		public DbSet<Guest> Guests { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderLine> OrderLines { get; set; }
		public DbSet<OrderCompletionText> OrderCompletionTexts { get; set; }
		public DbSet<Paymend> Paymends { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<ProductVariant> ProductVariants { get; set; }
		public DbSet<ProductVariantOption> ProductVariantOptions { get; set; }
		public DbSet<SellAction> SellActions { get; set; }
		public DbSet<SellActionItem> SellActionItems { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<ShoppingCartLine> ShoppingCartLines { get; set; }
		public DbSet<ShopFile> ShopFiles { get; set; }
		public DbSet<ShippingPeriod> ShpippingPeriods { get; set; }
		public DbSet<ShippingPrice> ShippingPrices { get; set; }
		public DbSet<ShippingPriceType> ShippingPriceTypes { get; set; }
		public DbSet<Size> Sizes { get; set; }
		public DbSet<Unit> Units { get; set; }
		public DbSet<ShippingAddress> ShippingAddresses { get; set; }
		public DbSet<Impressum> Impressums { get; set; }
		public DbSet<ShopContent> ShopContents { get; set; }
		public DbSet<LawContent> LawContents { get; set; }
		public DbSet<VariantProductAssignment> VariantProductAssignments { get; set; }
	}
}
