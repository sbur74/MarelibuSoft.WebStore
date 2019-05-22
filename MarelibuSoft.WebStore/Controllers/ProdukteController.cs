using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Models.ViewModels;
using MarelibuSoft.WebStore.Common.Helpers;
using Microsoft.Extensions.Logging;
using MarelibuSoft.WebStore.Common.Statics;
using MarelibuSoft.WebStore.Services;
using MarelibuSoft.WebStore.Extensions;

namespace MarelibuSoft.WebStore.Areas.Store.Controllers
{
    public class ProdukteController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly ILoggerFactory factory;
		private readonly ILogger logger;
		private ShoppingCartHelper cartHelper;
		private readonly IMetaService metaService;

		public ProdukteController(ApplicationDbContext context, ILoggerFactory loggerFactory, IMetaService metaService)
        {
            _context = context;
			factory = loggerFactory;
			logger = factory.CreateLogger<ProdukteController>();
			cartHelper = new ShoppingCartHelper(_context, factory.CreateLogger<ShoppingCartHelper>());
			this.metaService = metaService;
		}


		public async Task<IActionResult>Kategorie([FromQuery]string categoryId, [FromQuery]string categorySubId, [FromQuery]string categoryDetailId)
		{
			cartHelper.CheckAndRemove();

			string titel = string.Empty;

			string metadescription = string.Empty;
			string metakeywords = string.Empty;
			string categorieDescription = string.Empty;

			int catId = 0;
			int catSubId = 0;
			int catDetId = 0;

			if (categoryId != null)
			{
				string[] catsplit = categoryId.Split("-");
				catId = int.Parse(catsplit[0]);
			}
			if (categorySubId != null)
			{
				string[] catSubSplit = categorySubId.Split("-");
				catSubId = int.Parse(catSubSplit[0]);
			}
			if (categoryDetailId != null)
			{
				string[] catDetSplit = categoryDetailId.Split("-");
				catDetId = int.Parse(catDetSplit[0]);
			}

            var sellacitons = await _context.SellActions.Where(a => a.StartDate <= DateTime.Now && a.EndDate >= DateTime.Now).Include(i => i.SellActionItems).ToListAsync();


            var products = await _context.Products.Include(p => p.ImageList).Include(ca => ca.CategoryAssignments).OrderByDescending(p => p.ProductID).ToListAsync();
			List<Product> filterProducts = new List<Product>();
			List<ProductThumbnailsViewModel> thubnails = new List<ProductThumbnailsViewModel>();
			var categoryAssignments = new List<CategoryAssignment>();
			if (catId > 0)
			{
				var category = await _context.Categories.Where(c => c.ID == catId).SingleAsync();

				titel = category.Name;
				metadescription = category.SeoDescription;
				metakeywords = category.SeoKeywords;
				categorieDescription = category.HtmlDescription;

				if (catSubId > 0)
				{
					var categorySub = await _context.CategorySubs.Where(c => c.ID == catSubId).SingleAsync();

					titel += $"-{categorySub.Name}";
					metadescription = categorySub.SeoDescription;
					metakeywords = categorySub.SeoKeywords;
					categorieDescription = categorySub.HtmlDescription;

					if (catDetId > 0)
					{
						var categoryDetail = await _context.CategoryDetails.Where(c => c.ID == catDetId).SingleAsync();

						titel += $"-{categoryDetail.Name}";
						metadescription = categoryDetail.SeoDescription;
						metakeywords = categoryDetail.SeoKeywords;
						categorieDescription = categoryDetail.HtmlDescription;

						categoryAssignments = await _context.CategoryAssignments.Where(ca => ca.CategoryID == catId && ca.CategorySubID == catSubId && ca.CategoryDetailID == catDetId).ToListAsync();
					}
					else
					{
						categoryAssignments = await _context.CategoryAssignments.Where(ca => ca.CategoryID == catId && ca.CategorySubID == catSubId).ToListAsync();
					}
				}
				else
				{
					categoryAssignments = await _context.CategoryAssignments.Where(ca => ca.CategoryID == catId).ToListAsync();
				}

				if (categoryAssignments.Count > 0)
				{
					foreach (CategoryAssignment item in categoryAssignments)
					{
						var product = products.Where(p => p.ProductID == item.ProductID && p.MinimumPurchaseQuantity <= p.AvailableQuantity && p.IsActive).FirstOrDefault();

						if (product != null)
						{
							if (!filterProducts.Contains(product))
							{
								filterProducts.Add(product);
							}
						}
					}
				}
			}

            string metaSellActionKeyWord = string.Empty;
            string metaSellActionDescription = string.Empty;

			foreach (Product item in filterProducts)
			{
				List<string> urls = new List<string>();
				string baseUnit = string.Empty;
				string secondPriceUnit = string.Empty;
				ProductThumbnailsViewModel vmProduct = new ProductThumbnailsViewModel();
                SellActionItem sellActionItem = null;
                SellAction sellAction = null;

                foreach (var action in sellacitons)    
                {
                    var sellactionitem = action.SellActionItems.Where(i => i.FkProductID == item.ProductID).LastOrDefault();

                    if (sellactionitem != null)
                    {
                        sellAction = action;
                        sellActionItem = sellactionitem;
                        if (string.IsNullOrEmpty(metaSellActionKeyWord))
                        {
                            metaSellActionKeyWord = $", rabatt";
                            metaSellActionDescription = $"/ RABATTAKTION/ Rabatt {(int)action.Percent}%/ {action.ActionName}";
                        }
                        break;
                    }
                }

				baseUnit = new UnitHelper(_context, factory).GetUnitName(item.BasesUnitID);

				try
				{
					vmProduct.ProductID = item.ProductID;
					vmProduct.ProductNumber = item.ProductNumber;

                    vmProduct.Price = Math.Round(item.Price, 2);
                    vmProduct.OrgPrice = 0.00M;
                    vmProduct.SellActionItemId = 0;
                   
                    decimal SecondBasePrice = item.SecondBasePrice;

                    if (sellAction != null)
                    {
                        if (sellActionItem != null)
                        {
                            vmProduct.Price = Math.Round((item.Price - (item.Price * sellAction.Percent)/100) , 2);
                            vmProduct.OrgPrice = Math.Round(item.Price, 2);
                            SecondBasePrice = Math.Round((SecondBasePrice - (SecondBasePrice * sellAction.Percent) / 100), 2);
                            vmProduct.SellActionPrecent = (int)sellAction.Percent;
                            vmProduct.SellActionItemId = sellActionItem.SellActionItemID;
                        }
                    }

                    if (SecondBasePrice != 0.0M && item.SecondBaseUnit != 0)
                    {
                        string strUnit = new UnitHelper(_context, factory).GetUnitName(item.SecondBaseUnit);
                        secondPriceUnit = Math.Round(SecondBasePrice, 2).ToString() + " €/" + strUnit;
                    }

                    vmProduct.AvailableQuantity = item.AvailableQuantity;
					vmProduct.BasesUnit = baseUnit;
					vmProduct.BasesUnitID = item.BasesUnitID;
					vmProduct.Description = item.Description;
					vmProduct.MinimumPurchaseQuantity = item.MinimumPurchaseQuantity;
					vmProduct.Name = item.Name;
					vmProduct.ShippingTime = new ShippingPeriodHelper(_context).GetDescription(item.ShippingPeriod);
					vmProduct.Size = new SizeHelper(_context).GetName(item.Size);
					vmProduct.ShortDescription = item.ShortDescription;
					vmProduct.SecondPriceUnit = secondPriceUnit;
					vmProduct.SlugUrl = $"{item.ProductID}-{item.ProductNumber}-{FriendlyUrlHelper.ReplaceUmlaute(item.Name)}";
                    vmProduct.IsNew = CheckIsNewProduct(item.PublishedOn);

                }
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}

				foreach (ProductImage img in item.ImageList)
				{
					if (img.IsMainImage)
					{
						vmProduct.MainImageUrl = img.ImageUrl;
					}
					urls.Add(img.ImageUrl);
				}
				if (string.IsNullOrEmpty(vmProduct.MainImageUrl))
				{
					if (urls.Count > 0)
					{
						vmProduct.MainImageUrl = urls.First();
					}
					else
					{
						vmProduct.MainImageUrl = "noImage.svg";
					}
				}
				vmProduct.ImageUrls = urls;

				thubnails.Add(vmProduct);
			}

			ViewData["CategorieDescription"] = categorieDescription;
			ViewData["Title"] = titel;
            
			if (!string.IsNullOrWhiteSpace(metadescription))
			{
                metadescription += metaSellActionDescription;
				metaService.AddMetadata("description", metadescription); 
			}
			if (!string.IsNullOrWhiteSpace(metakeywords))
			{
                metakeywords += metaSellActionKeyWord;
				metaService.AddMetadata("keywords", metakeywords);
			}

			thubnails.Shuffle();
 
			return View(thubnails);
		}

		// GET: Store/Products/Artile/5
		public async Task<IActionResult> Artikel(string id)
        {
			cartHelper.CheckAndRemove();

			if (id == null)
            {
                return NotFound();
            }
			ViewData["Title"] = id;
			string[] split = id.Split("-");

			int productId = int.Parse(split[0]);

            var product = await _context.Products
				.Include(p => p.ImageList)
                .SingleOrDefaultAsync(m => m.ProductID == productId);
            if (product == null)
            {
                return NotFound();
            }

            List<ProductImage> imgs = product.ImageList;

            ProductImage mainImg = GetMainImageUrl(imgs);

            var sellacitons = await _context.SellActions.Where(a => a.StartDate <= DateTime.Now && a.EndDate >= DateTime.Now).Include(i => i.SellActionItems).ToListAsync();

            SellActionItem sellActionItem = null;
            SellAction sellAction = null;
            string metaSellActionKeyWord = string.Empty;
            string metaSellActionDescription = string.Empty;


            foreach (var action in sellacitons)
            {
                var sellactionitem = action.SellActionItems.Where(i => i.FkProductID == product.ProductID).LastOrDefault();

                if (sellactionitem != null)
                {
                    sellAction = action;
                    sellActionItem = sellactionitem;
                    if (string.IsNullOrEmpty(metaSellActionKeyWord))
                    {
                        metaSellActionKeyWord = $", rabatt";
                        metaSellActionDescription = $"/ RABATTAKTION/ Rabatt {(int)action.Percent}%/ {action.ActionName}";
                    }
                    break;
                }
            }

            decimal productPrice = Math.Round(product.Price, 2);
            decimal productOrgPrice = 0.0M;
            decimal productSellActionPrecent = 0.0M;
            int sellactionitemid = 0;
            decimal SecondBasePrice = Math.Round(product.SecondBasePrice, 2);

            if (sellAction != null)
            {
                if (sellActionItem != null)
                {
                    productPrice = Math.Round((productPrice - (productPrice * sellAction.Percent) / 100), 2);
                    productOrgPrice = Math.Round(product.Price, 2);
                    SecondBasePrice = Math.Round((SecondBasePrice - (SecondBasePrice * sellAction.Percent) / 100), 2);
                    productSellActionPrecent = Math.Round(sellAction.Percent, 2);
                    sellactionitemid = sellActionItem.SellActionItemID;
                }
            }

            string secondPriceUnit = "";
			if (SecondBasePrice != 0.0M && product.SecondBaseUnit != 0)
			{
				string strUnit = new UnitHelper(_context, factory).GetUnitName(product.SecondBaseUnit);
				secondPriceUnit = Math.Round( SecondBasePrice,2 ).ToString() + " €/" + strUnit;
			}

			string baseuint = new UnitHelper(_context, factory).GetUnitName(product.BasesUnitID);

			ProductDetailViewModel dvm = new ProductDetailViewModel()
			{
				ProductID = product.ProductID,
				AvailableQuantity = product.AvailableQuantity,
				BasesUnit = baseuint,
				BasesUnitID = product.BasesUnitID,
				Description = product.Description,
				Price = productPrice,
                OrgPrice = productOrgPrice,
                SellActionPrecent = (int) productSellActionPrecent,
                SellActionItemId = sellactionitemid,
				MinimumPurchaseQuantity = product.MinimumPurchaseQuantity,
				Name = product.Name,
				ProductNumber = product.ProductNumber,
				ShippingTime = new ShippingPeriodHelper(_context).GetDescription(product.ShippingPeriod),
				ShortDescription = product.ShortDescription,
				Size = new SizeHelper(_context).GetName(product.Size),
				ImageUrls = imgs,
				MainImageUrl = mainImg,
				SecondPriceUnit = secondPriceUnit,
				SeoDescription = product.SeoDescription,
				SeoKeywords = product.SeoKeywords,
				Shipping = new ShippingPriceTypeHelper(_context).GetNameByID(product.ShippingPriceType),
                IsNew = CheckIsNewProduct(product.PublishedOn)
				
			};

			if (!string.IsNullOrWhiteSpace(dvm.SeoDescription))
			{
                dvm.SeoDescription += metaSellActionDescription;
				metaService.AddMetadata("description", dvm.SeoDescription); 
			}
			if (!string.IsNullOrWhiteSpace(dvm.SeoKeywords))
			{
                dvm.SeoKeywords += metaSellActionKeyWord;
				metaService.AddMetadata("keywords", dvm.SeoKeywords);
			}

			return View(dvm);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

		private ProductImage GetMainImageUrl(List<ProductImage> images)
		{
			ProductImage restult = new ProductImage { ImageUrl = "noImage.svg", Name = "Kein Bild zugeordent" };

			foreach (ProductImage item in images)
			{
				if (item.IsMainImage)
				{
					restult = item;
					break;
				}
			}

			if (restult.ImageUrl.Equals("noImage.svg") && images.Count >0)
			{
				restult = images.First();
			}
			return restult;
		}

        private bool CheckIsNewProduct(DateTime publisheddate)
        {
            bool isNew = false;

            DateTime now = DateTime.Now;
            TimeSpan timeSpan = now.Subtract(publisheddate);

            if (timeSpan.TotalDays < 14)
            {
                isNew = true;
            }

            return isNew;
        }
    }
}
