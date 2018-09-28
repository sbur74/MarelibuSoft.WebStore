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

			if (addresses.Count == 0)
			{
				Customer customer = await _context.Customers.SingleAsync(c => c.CustomerID == id);
				
				ShippingAddress main = new ShippingAddress() { CustomerID = customer.CustomerID, AdditionalAddress = customer.AdditionalAddress, Address = customer.Address, City = customer.City, FirstName = customer.FirstName, IsMainAddress = true, LastName = customer.Name, PostCode = customer.PostCode };

				_context.Add(main);
				await _context.SaveChangesAsync();

				addresses = await _context.ShippingAddresses.Where(sh => sh.CustomerID.Equals(id)).ToListAsync();
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
					CountryName = new CountryHelper(_context).GetNameByID(item.CountryID),
					CustomerID = item.CustomerID,
					PostCode = item.PostCode,
					FirstName = item.FirstName,
					IsMainAddress = item.IsMainAddress,
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
			if (id != null)
			{
				var address = new ShippingAddress { CustomerID = (Guid)id };
				return View(address);
			}
			ViewData["CountryID"] = new SelectList(new CountryHelper(_context).GetVmList());

			return View();
        }

        // POST: ShippingAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Address,AdditionalAddress,City,PostCode,Customer,CountryID")] ShippingAddress shippingAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shippingAddress);
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

			ViewData["CountryID"] = new SelectList(new CountryHelper(_context).GetVmList(shippingAddress.CountryID));

			return View(shippingAddress);
        }

        // POST: ShippingAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Address,AdditionalAddress,City,PostCode,Customer,CountryID")] ShippingAddress shippingAddress)
        {
            if (id != shippingAddress.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingAddress);
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
                return RedirectToAction(nameof(Index));
            }
            return View(shippingAddress);
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
            _context.ShippingAddresses.Remove(shippingAddress);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingAddressExists(int id)
        {
            return _context.ShippingAddresses.Any(e => e.ID == id);
        }
    }
}
