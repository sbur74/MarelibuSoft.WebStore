using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Models.ViewModels;

namespace MarelibuSoft.WebStore.Common.Helpers
{
	public class ShippingAddressHelper
	{
		private readonly ApplicationDbContext _context;
		private CountryHelper countryHelper;

		public ShippingAddressHelper(ApplicationDbContext context)
		{
			_context = context;
			countryHelper = new CountryHelper(_context);
		}

		public void UpdateMainAsync(int id, bool isMail)
		{
			var address =  _context.ShippingAddresses.Single(s => s.ID == id);
			address.IsMainAddress = isMail;
			_context.Update(address);
			_context.SaveChangesAsync();
		}

		public ShippingAddressViewModel GetViewModel(ShippingAddress address)
		{
			var countryName = countryHelper.GetNameByID(address.CountryID);
			return new ShippingAddressViewModel { ID = address.ID, AdditionalAddress = address.AdditionalAddress, CustomerID = address.CustomerID, CountryID = address.CountryID, CountryName = countryName, Address = address.Address, City = address.City, CompanyName = address.CompanyName, FirstName = address.FirstName, IsInvoiceAddress = address.IsInvoiceAddress, IsMainAddress = address.IsMainAddress, LastName = address.LastName, PostCode = address.PostCode };
		}

		public ShippingAddressViewModel GetViewModel(Customer customer)
		{
			var countryName = countryHelper.GetNameByID(customer.CountryId);
			return new ShippingAddressViewModel { ID = 0, PostCode = customer.PostCode, LastName = customer.Name, IsMainAddress = false, IsInvoiceAddress = true, AdditionalAddress = customer.AdditionalAddress, Address = customer.Address, City = customer.City, CompanyName = customer.CompanyName, CountryID = customer.CountryId, CountryName = countryName, CustomerID = customer.CustomerID, FirstName = customer.FirstName };
		}
	}
}
