using MarelibuSoft.WebStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarelibuSoft.WebStore.Models;

namespace MarelibuSoft.WebStore.Common.Helpers
{
	internal class ShoppingCartHelper
	{
		private readonly ApplicationDbContext context;
		private readonly ILogger logger;
		private const long maxLifeTimeTicks = 15000000000;

		public ShoppingCartHelper(ApplicationDbContext dbContext, ILogger<ShoppingCartHelper>log)
		{
			context = dbContext;
			logger = log;
		}

		public void RemoveCart(Guid id)
		{
			try
			{
				var cart = context.ShoppingCarts.Single(c => c.ID.Equals(id));

				if(cart != null)
				{
					var lines = context.ShoppingCartLines
                        .Where(l => l.ShoppingCartID.Equals(cart.ID))
                        .Include(o => o.VariantValues)
                        .ToList();
					foreach (var item in lines)
					{
						var product = context.Products
                            .Include(v => v.ProductVariants)
                            .ThenInclude(o => o.Options)
                            .Single(p => p.ProductID == item.ProductID);

                        if(item.VariantValues.Count > 0)
                        {
                            foreach(ShoppingCartLineVariantValue variantValue in item.VariantValues)
                            {
                                var option = product.ProductVariants
                                    .Single(v => v.ID == variantValue.ProductVariant)
                                    .Options.Single(o => o.ID == variantValue.ProductVariantOption);
                                option.Quantity += variantValue.Quantity;
                                context.ProductVariantOptions.Update(option);
                            }
                        }
                        else
                        {
                            product.AvailableQuantity += item.Quantity;
                        }
						
						context.Products.Update(product);
						context.ShoppingCartLines.Remove(item);
					}
					context.Remove(cart);
					
				}

				context.SaveChanges();
			}
			catch (Exception e)
			{
				logger.LogError(e, $"ShoppingCartHelper.RemoveCart -> Fehler beim löschen von Warenkorb {id}");
			}
		}

		public void CheckAndRemove()
		{
			try
			{
				var carts = context.ShoppingCarts.ToList();

				foreach (var cart in carts)
				{
					
					if (cart != null)
					{
						TimeSpan diff = DateTime.Now - cart.LastChange;
						if (diff.Ticks > maxLifeTimeTicks)
						{
							var lines = context.ShoppingCartLines.Where(l => l.ShoppingCartID.Equals(cart.ID)).ToList();
							foreach (var item in lines)
							{
								var product = context.Products.Single(p => p.ProductID == item.ProductID);
								product.AvailableQuantity += item.Quantity;

								context.Products.Update(product);
								context.ShoppingCartLines.Remove(item);
							}
							context.Remove(cart); 
						}

					} 
				}

				context.SaveChanges();
			}
			catch (Exception e)
			{
				logger.LogError(e, $"ShoppingCartHelper.CheckAndRemove -> Fehler beim löschen von Warenkörben");
			}
		}
	}
}
