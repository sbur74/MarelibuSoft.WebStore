using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarelibuSoft.WebStore.Data;
using MarelibuSoft.WebStore.Models;
using MarelibuSoft.WebStore.Common.Helpers;
using MarelibuSoft.WebStore.Models.ViewModels;

namespace MarelibuSoft.WebStore.Controllers
{
    public class ShippingAddressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShippingAddresses
        public async Task<IActionResult> Index(Guid? custId)
        {
			if (custId != null)
			{
				ViewData["CustomerID"] = custId;
				return View(await _context.ShippingAddresses.Where(sh => sh.CustomerID.Equals(custId)).ToListAsync());
			}
            return View(await _context.ShippingAddresses.ToListAsync());
        }

		public async Task<IActionResult> CustomerIndex(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			ViewData["CustomerID"] = id;
			List<ShippingAddress> addresses = new List<ShippingAddress>();
			try
			{
				addresses = await _context.ShippingAddresses.Where(sh => sh.CustomerID == id).ToListAsync();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			List<CustomerShipToAddressVm> vms = new List<CustomerShipToAddressVm>();

			foreach (var item in addresses)
			{
				CustomerShipToAddressVm vm = new CustomerShipToAddressVm
				{
					ID = item.ID,
					AdditionalAddress = item.AdditionalAddress,
					Address = item.Address,
					City = item.City,
					CountryID = item.CountryID,
					CompanyName = item.CompanyName,
					CountryName = new CountryHelper(_context).GetNameByID(item.CountryID),
					CustomerID = item.CustomerID,
					PostCode = item.PostCode,
					FirstName = item.FirstName,
					IsMainAddress = item.IsMainAddress,
					IsInvoiceAddress = item.IsInvoiceAddress,
					LastName = item.LastName
				};

				vms.Add(vm);
			}

			return View(vms);
		}

        // GET: ShippingAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingAddress = await _context.ShippingAddresses
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shippingAddress == null)
            {
                return NotFound();
            }

            return View(shippingAddress);
        }

        // GET: ShippingAddresses/Create
        public IActionResult Create(Guid? id)
        {
			var countries = new CountryHelper(_context).GetShippingCountries(1); //default 1 deutschland
			ViewData["Countries"] = new SelectList(countries, "ID", "Name");
			ViewData["CustomerId"] = id;
			
			return View();
        }

        // POST: ShippingAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Address,AdditionalAddress,City,PostCode,IsMainAddress,CustomerID,CountryID,CompanyName")] ShippingAddress shippingAddress)
		{
            if (ModelState.IsValid)
            {
				if (shippingAddress.IsMainAddress)
				{
					var addresses = await _context.ShippingAddresses.Where(a => a.CustomerID.Equals(shippingAddress.CustomerID)).ToListAsync();
					foreach (var item in addresses)
					{
						item.IsMainAddress = false;
					}
					_context.UpdateRange(addresses);
				}
				_context.Add(shippingAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CustomerIndex), new { id = shippingAddress.CustomerID });
            }
			return RedirectToAction(nameof(Create), new { shippingAddress.CustomerID });
        }

        // GET: ShippingAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			

            var shippingAddress = await _context.ShippingAddresses.SingleOrDefaultAsync(m => m.ID == id);
            if (shippingAddress == null)
            {
                return NotFound();
            }

			var countries = new CountryHelper(_context).GetShippingCountries(shippingAddress.CountryID);

			ViewData["CountryID"] = new SelectList(countries,"ID","Name");

			return View(shippingAddress);
        }

        // POST: ShippingAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Address,AdditionalAddress,City,PostCode,IsMainAddress,CustomerID,CountryID,CompanyName")] ShippingAddress shippingAddress)
		{
            if (id != shippingAddress.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					if (shippingAddress.IsMainAddress)
					{
						var addresses = await _context.ShippingAddresses.Where(a => a.CustomerID.Equals(shippingAddress.CustomerID)).ToListAsync();
						foreach (var item in addresses)
						{
							if (item.ID != shippingAddress.ID)
							{
								item.IsMainAddress = false;
							}
							else
							{
								item.IsMainAddress = shippingAddress.IsMainAddress;
								item.FirstName = shippingAddress.FirstName;
								item.LastName = shippingAddress.LastName;
								item.PostCode = shippingAddress.PostCode;
								item.City = shippingAddress.City;
								item.CountryID = shippingAddress.CountryID;
								item.Address = shippingAddress.Address;
								item.AdditionalAddress = shippingAddress.AdditionalAddress;
							}
						}
						_context.UpdateRange(addresses);
					}
					else
					{
						_context.Update(shippingAddress);
					}

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingAddressExists(shippingAddress.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CustomerIndex), new { id = shippingAddress.CustomerID});
            }
			return RedirectToAction(nameof(Edit), new { shippingAddress.ID });
		}

        // GET: ShippingAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingAddress = await _context.ShippingAddresses
                .SingleOrDefaultAsync(m => m.ID == id);
            if (shippingAddress == null)
            {
                return NotFound();
            }

            return View(shippingAddress);
        }

        // POST: ShippingAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shippingAddress = await _context.ShippingAddresses.SingleOrDefaultAsync(m => m.ID == id);
			var custID = shippingAddress.CustomerID;
            _context.ShippingAddresses.Remove(shippingAddress);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CustomerIndex), new { id = custID });
        }
		
		public async Task<IActionResult> UseAddress(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var address = await _context.ShippingAddresses.SingleAsync(s => s.ID == id);
			Guid customerID = address.CustomerID;
			var addresses = await _context.ShippingAddresses.Where(a => a.CustomerID.Equals(customerID)).ToListAsync();

			foreach (var item in addresses)
			{
				if (item.ID == id) item.IsMainAddress = true;
				else item.IsMainAddress = false;				
			}
			_context.UpdateRange(addresses);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(CustomerIndex), new { id = customerID });
		}


		private bool ShippingAddressExists(int id)
        {
            return _context.ShippingAddresses.Any(e => e.ID == id);
        }
    }
}
