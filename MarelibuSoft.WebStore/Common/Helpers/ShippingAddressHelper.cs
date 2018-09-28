using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MarelibuSoft.WebStore.Common.Helpers
{
	public class ShippingAddressHelper
	{
		private readonly ApplicationDbContext _context;

		public ShippingAddressHelper(ApplicationDbContext context)
		{
			_context = context;
		}

		public void UpdateMainAsync(int id, bool isMail)
		{
			var address =  _context.ShippingAddresses.Single(s => s.ID == id);
			address.IsMainAddress = isMail;
			_context.Update(address);
			_context.SaveChangesAsync();
		}
	}
}
