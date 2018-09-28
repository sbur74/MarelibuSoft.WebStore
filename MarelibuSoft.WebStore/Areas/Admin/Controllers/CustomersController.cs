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
using MarelibuSoft.WebStore.Areas.Admin.Models.AdminViewModels;
using MarelibuSoft.WebStore.Models.ViewModels;

namespace MarelibuSoft.WebStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Customers
        public async Task<IActionResult> Index()
        {
			ViewData["CountryID"] = new SelectList(new CountryHelper(_context).GetVmList(), "ID", "Name");
			var customers = await _context.Customers.ToListAsync();
			var vms = new List<AdminCustomerViewModel>();

			foreach (var item in customers)
			{
				var user = await _context.Users.SingleAsync(u => u.Id==item.UserId);
				var country = await _context.Countries.SingleAsync(c => c.ID == item.CountryId);

				var custVm = new AdminCustomerViewModel
				{
					CustomerID = item.CustomerID,
					CustomerNo = item.CustomerNo,
					AdditionalAddress = item.AdditionalAddress,
					Address = item.Address,
					Addresses = null,
					AllowedPayByBill = item.AllowedPayByBill,
					City = item.City,
					CountryName = country.Name,
					CountryId = item.CountryId,
					Name = item.Name,
					FirstName = item.FirstName,
					PostCode = item.PostCode,
					UserEmail = user.Email,
					UserId = item.UserId
				};
				vms.Add(custVm);
			}

			return View(vms);
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .SingleOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }
			var vm = await GetCustomerViewModel((Guid)id);

			return View(vm);
        }

        // GET: Admin/Customers/Create
        public IActionResult Create()
        {
			ViewData["CountryID"] = new SelectList(new CountryHelper(_context).GetVmList(), "ID", "Name");
			return View();
        }

        // POST: Admin/Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,CustomerNo,FirstName,Name,Address,AdditionalAddress,City,PostCode,DateOfBirth,AllowedPayByBill,UserId,CountryId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CustomerID = Guid.NewGuid();
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Admin/Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

			var vm = await GetCustomerViewModel((Guid)id);

			ViewData["CountryID"] = new SelectList(new CountryHelper(_context).GetVmList(), "ID", "Name");
			return View(vm);
        }

        // POST: Admin/Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CustomerID,CustomerNo,FirstName,Name,Address,AdditionalAddress,City,PostCode,DateOfBirth,AllowedPayByBill,UserId,CountryId")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            return View(customer);
        }

        // GET: Admin/Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .SingleOrDefaultAsync(m => m.CustomerID == id);


            if (customer == null)
            {
                return NotFound();
            }
			var vm =await GetCustomerViewModel((Guid)id);

			return View(vm);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.CustomerID == id);
			var user = await _context.Users.SingleAsync(u => u.Id == customer.UserId);
            _context.Customers.Remove(customer);
			_context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		private async Task<AdminCustomerViewModel> GetCustomerViewModel(Guid id)
		{
			AdminCustomerViewModel result = new AdminCustomerViewModel();

			var customer = await _context.Customers.SingleAsync(c => c.CustomerID == id);
			var user = await _context.Users.SingleAsync(u => u.Id == customer.UserId);
			var countries = await _context.Countries.ToListAsync();
			var shipToAddresses = await _context.ShippingAddresses.Where(sh => sh.CustomerID == customer.CustomerID).ToListAsync();

			result.CustomerID = customer.CustomerID;
			result.CustomerNo = customer.CustomerNo;
			result.FirstName = customer.FirstName;
			result.Name = customer.Name;
			result.PostCode = customer.PostCode;
			result.UserEmail = user.Email;
			result.UserId = user.Id;
			result.City = customer.City;
			result.AdditionalAddress = customer.AdditionalAddress;
			result.Address = customer.Address;
			result.AllowedPayByBill = customer.AllowedPayByBill;
			result.Addresses = new List<ShippingAddressViewModel>();
			result.CountryId = customer.CountryId;
			result.CountryName = countries.Single(c => c.ID == customer.CountryId).Name;

			foreach (var item in shipToAddresses)
			{
				var shipTo = new ShippingAddressViewModel
				{
					ID = item.ID,
					Address = item.Address,
					AdditionalAddress = item.AdditionalAddress,
					City = item.City,
					CountryID = item.CountryID,
					CountryName = countries.Single(c => c.ID == item.CountryID).Name,
					CustomerID = item.CustomerID,
					FirstName = item.FirstName,
					IsMainAddress = item.IsMainAddress,
					LastName = item.LastName,
					PostCode = item.PostCode,
					IsInvoiceAddress = item.IsInvoiceAddress
				};
				result.Addresses.Add(shipTo);
				
			}
		

			return result;
		}

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
